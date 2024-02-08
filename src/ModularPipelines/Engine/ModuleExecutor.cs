using System.Collections.Concurrent;
using System.Reflection;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Engine;

internal class ModuleExecutor : IModuleExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISafeModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly IModuleDisposer _moduleDisposer;

    private readonly ConcurrentDictionary<ModuleBase, Task<ModuleBase>> _moduleExecutionTasks = new();

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _moduleDisposer = moduleDisposer;
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
            return await ExecuteWithLockAsync(module);
        }
        catch (TaskCanceledException)
        {
            // If the pipeline failed, sometimes a TaskCanceledException can throw before the original exception
            // So delay a bit to let the original exception throw first
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            throw;
        }
    }

    private Task<ModuleBase> ExecuteWithLockAsync(ModuleBase module)
    {
        lock (module)
        {
            return _moduleExecutionTasks.GetOrAdd(module, @base => StartModule(module));
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
        if (module.IsStarted || module.ExecutionTask.IsCompleted)
        {
            await module.ExecutionTask;
            return module;
        }

        try
        {
            await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

            await module.ExecutionTask;

            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

            await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

            return module;
        }
        finally
        {
            if (!AnsiConsole.Profile.Capabilities.Interactive || !_pipelineOptions.Value.ShowProgressInConsole)
            {
                await _moduleDisposer.DisposeAsync(module);
            }
        }
    }

    private async Task<IEnumerable<ModuleBase>> ProcessGroup(
        IGrouping<string?, ModuleBase> moduleBases,
        ICollection<ModuleBase> moduleResults)
    {
        var executionProcessor = moduleBases.SelectAsync(ExecuteAsync);

        if (string.IsNullOrEmpty(moduleBases.Key))
        {
            return await executionProcessor.ProcessInParallel().GetEnumerableTasks().ToArray().WhenAllFailFast();
        }

        return await executionProcessor.ProcessOneAtATime();
    }
}
