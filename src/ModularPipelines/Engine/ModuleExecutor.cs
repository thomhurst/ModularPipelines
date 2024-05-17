using System.Collections.Concurrent;
using System.Reflection;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleExecutor : IModuleExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISafeModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IEnumerable<ModuleBase> _allModules;
    private readonly IExceptionContainer _exceptionContainer;

    private readonly ConcurrentDictionary<ModuleBase, Task<ModuleBase>> _moduleExecutionTasks = new();

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer,
        IEnumerable<ModuleBase> allModules,
        IExceptionContainer exceptionContainer)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _moduleDisposer = moduleDisposer;
        _allModules = allModules;
        _exceptionContainer = exceptionContainer;
    }

    public async Task<IEnumerable<ModuleBase>> ExecuteAsync(IReadOnlyList<ModuleBase> modules)
    {
        var moduleResults = new List<ModuleBase>();

        var nonParallelModules = modules
            .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>() != null)
            .OrderBy(x => x.DependentModules.Count)
            .ToList();

        var unKeyedNonParallelModules = nonParallelModules
            .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKeys.Length == 0)
            .ToList();

        foreach (var nonParallelModule in unKeyedNonParallelModules)
        {
            moduleResults.Add(await ExecuteAsync(nonParallelModule));
        }

        var keyedNonParallelModules = nonParallelModules
            .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKeys.Length != 0)
            .ToList();

        moduleResults.AddRange(
            await ProcessKeyedNonParallelModules(keyedNonParallelModules.ToList(), moduleResults)
        );

        var parallelModuleTasks = modules.Except(nonParallelModules)
            .Select(ExecuteAsync)
            .ToArray();

        if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
        {
            moduleResults.AddRange(await parallelModuleTasks.WhenAllFailFast());
        }
        else
        {
            moduleResults.AddRange(await Task.WhenAll(parallelModuleTasks));
        }

        return moduleResults;
    }

    public async Task<ModuleBase> ExecuteAsync(ModuleBase module)
    {
        try
        {
            return await StartModule(module);
        }
        catch (TaskCanceledException)
        {
            // If the pipeline failed, sometimes a TaskCanceledException can throw before the original exception
            // So delay a bit to let the original exception throw first
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            throw;
        }
    }

    private async Task<ModuleBase[]> ProcessKeyedNonParallelModules(List<ModuleBase> keyedNonParallelModules,
        List<ModuleBase> moduleResults)
    {
        var currentlyExecutingByKeysLock = new object();
        var currentlyExecutingByKeys = new List<(string[] Keys, Task)>();

        var executing = new List<Task<ModuleBase>>();

        while (keyedNonParallelModules.Count > 0)
        {
            // Reversing allows us to remove from the collection
            for (var i = keyedNonParallelModules.Count - 1; i >= 0; i--)
            {
                var module = keyedNonParallelModules[i];

                var notInParallelKeys =
                    module.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKeys;

                lock (currentlyExecutingByKeysLock)
                {
                    if (currentlyExecutingByKeys.Any(x => x.Keys.Intersect(notInParallelKeys).Any()))
                    {
                        // There are currently executing tasks with that same 
                        continue;
                    }
                }

                // Remove from collection as we're now processing it
                keyedNonParallelModules.RemoveAt(i);

                var executionTask = ExecuteAsync(module);

                var tuple = (notInParallelKeys, executionTask);

                lock (currentlyExecutingByKeysLock)
                {
                    currentlyExecutingByKeys.Add(tuple);
                }

                _ = executionTask.ContinueWith(_ =>
                {
                    lock (currentlyExecutingByKeysLock)
                    {
                        return currentlyExecutingByKeys.Remove(tuple);
                    }
                });

                executing.Add(executionTask);
            }
        }

        return await Task.WhenAll(executing);
    }

    private async Task<ModuleBase> StartModule(ModuleBase module)
    {
        return await _moduleExecutionTasks.GetOrAdd(module, async @base =>
        {
            var dependencies = module.GetModuleDependencies();

            foreach (var dependency in dependencies)
            {
                await StartDependency(module, dependency.DependencyType, dependency.IgnoreIfNotRegistered);
            }

            try
            {
                await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

                await module.StartInternal();

                await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

                await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

                return module;
            }
            finally
            {
                if (!_pipelineOptions.Value.ShowProgressInConsole)
                {
                    await _moduleDisposer.DisposeAsync(module);
                }
            }
        });
    }

    private async Task StartDependency(ModuleBase requestingModule, Type dependencyType, bool ignoreIfNotRegistered)
    {
        var module = _allModules.FirstOrDefault(x => x.GetType() == dependencyType);

        if (module is null && ignoreIfNotRegistered)
        {
            requestingModule.Context.Logger.LogDebug("{Module} was not registered so not waiting", dependencyType.Name);
            return;
        }

        if (module is null)
        {
            throw new ModuleNotRegisteredException($"The module {dependencyType.Name} has not been registered", null);
        }
        
        requestingModule.Context.Logger.LogDebug("{RequestingModule} is waiting for {Module}", requestingModule.GetType().Name, dependencyType.Name);

        try
        {
            await StartModule(module);
        }
        catch (Exception e) when (requestingModule.ModuleRunType == ModuleRunType.AlwaysRun)
        {
            _exceptionContainer.RegisterException(new AlwaysRunPostponedException($"{dependencyType.Name} threw an exception when {requestingModule.GetType().Name} was waiting for it as a dependency", e));
            requestingModule.Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
        }
        catch (DependencyFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new DependencyFailedException(e, module);
        }
    }
}
