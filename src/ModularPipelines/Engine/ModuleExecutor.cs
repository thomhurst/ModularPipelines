using System.Collections.Concurrent;
using System.Reflection;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
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
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly ILogger<ModuleExecutor> _logger;

    private readonly ConcurrentDictionary<ModuleBase, Task<ModuleBase>> _moduleExecutionTasks = new();
    private readonly object _moduleDictionaryLock = new();

    private readonly ConcurrentDictionary<string, Semaphore> _notInParallelKeyedLocks = new();
    private readonly object _notInParallelDictionaryLock = new();
    
    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer,
        IEnumerable<ModuleBase> allModules,
        IExceptionContainer exceptionContainer,
        IParallelLimitProvider parallelLimitProvider,
        ILogger<ModuleExecutor> logger)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _moduleDisposer = moduleDisposer;
        _allModules = allModules;
        _exceptionContainer = exceptionContainer;
        _parallelLimitProvider = parallelLimitProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<ModuleBase>> ExecuteAsync(IReadOnlyList<ModuleBase> modules)
    {
        try
        {
            var nonParallelModules = modules
                .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>() != null)
                .OrderBy(x => x.DependentModules.Count)
                .ToList();

            var unKeyedNonParallelModules = nonParallelModules
                .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKeys.Length == 0)
                .ToList();

            foreach (var nonParallelModule in unKeyedNonParallelModules)
            {
                await StartModule(nonParallelModule, false);
            }

            var keyedNonParallelModules = nonParallelModules
                .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKeys.Length != 0)
                .ToList();
            
            await ProcessKeyedNonParallelModules(keyedNonParallelModules.ToList());

            var parallelModuleTasks = modules.Except(nonParallelModules)
                .Select(x => StartModule(x, false))
                .ToArray();

            if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
            {
                await parallelModuleTasks.WhenAllFailFast();
            }
            else
            {
                await Task.WhenAll(parallelModuleTasks);
            }

            return modules;
        }
        catch
        {
            foreach (var moduleBase in modules.Where(x => x.ModuleRunType == ModuleRunType.AlwaysRun))
            {
                try
                {
                    await StartModule(moduleBase, false);
                }
                catch
                {
                    // Ignored
                }
            }

            throw;
        }
    }

    private Semaphore GetLockForKey(string key)
    {
        lock (_notInParallelDictionaryLock)
        {
            return _notInParallelKeyedLocks.GetOrAdd(key, _ => new Semaphore(1, 1));
        }
    }

    private async Task ProcessKeyedNonParallelModules(List<ModuleBase> keyedNonParallelModules)
    {
        await keyedNonParallelModules
            .OrderBy(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKeys.Length)
            .ForEachAsync(async module =>
            {
                var keys = module.GetType()
                    .GetCustomAttribute<NotInParallelAttribute>()!
                    .ConstraintKeys
                    .OrderBy(x => x)
                    .ToArray();
                
                _logger.LogDebug("Grabbing not in parallel locks for keys {Keys}", string.Join(", ", keys));
                
                var locks = keys.Select(GetLockForKey).ToArray();
                
                while (!WaitHandle.WaitAll(locks, TimeSpan.FromMilliseconds(100), false))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                }

                try
                {
                    await StartModule(module, false);
                }
                finally
                {
                    foreach (var semaphore in locks)
                    {
                        semaphore.Release();
                    }
                }
            })
            .ProcessInParallel();
    }

    private Task<ModuleBase> StartModule(ModuleBase module, bool isStartedAsDependencyForOtherModule)
    {
        lock (_moduleDictionaryLock)
        {
            return _moduleExecutionTasks.GetOrAdd(module, @base => Task.Run(async () =>
            {
                using var semaphoreHandle = await WaitForParallelLimiter(module, isStartedAsDependencyForOtherModule);
                
                _logger.LogDebug("Starting Module {Module}", module.GetType().Name);

                var dependencies = module.GetModuleDependencies();

                foreach (var dependency in dependencies)
                {
                    await StartDependency(module, dependency.DependencyType, dependency.IgnoreIfNotRegistered);
                }

                try
                {
                    ModuleLogger.Values.Value = module.Context.Logger;
                    
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
            }));
        }
    }
    
    private async Task<IDisposable> WaitForParallelLimiter(ModuleBase module, bool isStartedAsDependencyForAnotherTest)
    {
        var parallelLimitAttributeType =
            module.GetType().GetCustomAttributes<ParallelLimiterAttribute>().FirstOrDefault()?.Type;
        
        if (parallelLimitAttributeType != null)
        {
            return await _parallelLimitProvider.GetLock(parallelLimitAttributeType).WaitAsync();
        }

        return NoOpDisposable.Instance;
    }

    private async Task StartDependency(ModuleBase requestingModule, Type dependencyType, bool ignoreIfNotRegistered)
    {
        _logger.LogDebug("Starting Dependency {Dependency} for Module {Module}", dependencyType.Name, requestingModule.GetType().Name);
        
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
            await StartModule(module, true);
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
