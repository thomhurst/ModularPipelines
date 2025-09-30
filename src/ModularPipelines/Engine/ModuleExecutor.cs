using System.Collections.Concurrent;
using System.Reflection;
using EnumerableAsyncProcessor.Extensions;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Events;
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
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly IMediator _mediator;
    private readonly ILogger<ModuleExecutor> _logger;

    private readonly ConcurrentDictionary<string, Semaphore> _notInParallelKeyedLocks = new();
    private readonly object _notInParallelDictionaryLock = new();

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer,
        IEnumerable<ModuleBase> allModules,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IParallelLimitProvider parallelLimitProvider,
        IMediator mediator,
        ILogger<ModuleExecutor> logger)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _moduleDisposer = moduleDisposer;
        _allModules = allModules;
        _secondaryExceptionContainer = secondaryExceptionContainer;
        _parallelLimitProvider = parallelLimitProvider;
        _mediator = mediator;
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
                .Select(x => Task.Factory.StartNew(() => StartModule(x, false), TaskCreationOptions.LongRunning))
                .Select(x => x.Unwrap())
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
                var moduleTask = StartModule(moduleBase, false);
                try
                {
                    await moduleTask;
                }
                catch
                {
                    // Ignored - but observe the exception to prevent unobserved task exceptions
                    _ = moduleTask.Exception;
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
            .ForEachAsync(module => Task.Run(async () =>
            {
                var keys = module.GetType()
                    .GetCustomAttribute<NotInParallelAttribute>()!
                    .ConstraintKeys
                    .OrderBy(x => x)
                    .ToArray();

                _logger.LogDebug("Acquiring parallel locks for keys: {Keys}", string.Join(", ", keys));

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
            }))
            .ProcessInParallel();
    }

    private Task<ModuleBase> StartModule(ModuleBase module, bool isStartedAsDependencyForOtherModule)
    {
        // Use the module's built-in execution tracking instead of maintaining our own dictionary
        var task = module.GetOrStartExecutionTask(async () =>
            {
                using var semaphoreHandle = await WaitForParallelLimiter(module, isStartedAsDependencyForOtherModule);

                var moduleName = MarkupFormatter.FormatModuleName(module.GetType().Name);
                _logger.LogDebug("{Icon} Starting module {ModuleName}", MarkupFormatter.PlayIcon, moduleName);

                var dependencies = module.GetModuleDependencies();

                foreach (var dependency in dependencies.Reverse())
                {
                    await StartDependency(module, dependency.DependencyType, dependency.IgnoreIfNotRegistered);
                }

                try
                {
                    ModuleLogger.Values.Value = module.Context.Logger;

                    await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

                    // Get estimated duration for this module
                    var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(module.GetType());

                    // Publish module started event
                    await _mediator.Publish(new ModuleStartedNotification(module, estimatedDuration));

                    await module.StartInternal();

                    // Check if module was skipped
                    if (module.Status == Enums.Status.Skipped)
                    {
                        await _mediator.Publish(new ModuleSkippedNotification(module, module.SkipResult));
                        return;
                    }

                    await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

                    await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

                    // Publish module completed event
                    var isSuccessful = module.Status == Enums.Status.Successful;
                    await _mediator.Publish(new ModuleCompletedNotification(module, isSuccessful));

                    return;
                }
                finally
                {
                    if (!_pipelineOptions.Value.ShowProgressInConsole)
                    {
                        await _moduleDisposer.DisposeAsync(module);
                    }
                }
            });

        return task;
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
        var dependencyName = MarkupFormatter.FormatModuleName(dependencyType.Name);
        var requestingModuleName = MarkupFormatter.FormatModuleName(requestingModule.GetType().Name);

        _logger.LogDebug("Starting dependency {Dependency} for module {Module}", dependencyName, requestingModuleName);

        var module = _allModules.FirstOrDefault(x => x.GetType() == dependencyType);

        if (module is null && ignoreIfNotRegistered)
        {
            requestingModule.Context.Logger.LogDebug("Module {Module} was not registered, skipping", dependencyName);
            return;
        }

        if (module is null)
        {
            throw new ModuleNotRegisteredException($"The module {dependencyType.Name} has not been registered", null);
        }

        requestingModule.Context.Logger.LogDebug("{RequestingModule} is waiting for {Module}", requestingModuleName, dependencyName);

        var moduleTask = StartModule(module, true);

        try
        {
            await moduleTask;
        }
        catch (Exception e) when (requestingModule.ModuleRunType == ModuleRunType.AlwaysRun)
        {
            _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                $"{dependencyType.Name} threw an exception when {requestingModule.GetType().Name} was waiting for it as a dependency",
                e));
            requestingModule.Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");

            // Observe the task's exception to prevent unobserved task exceptions
            _ = moduleTask.Exception;
        }
        catch (DependencyFailedException e)
        {
            _secondaryExceptionContainer.RegisterException(e);
        }
        catch (PipelineCancelledException e)
        {
            _secondaryExceptionContainer.RegisterException(e);
        }
    }
}
