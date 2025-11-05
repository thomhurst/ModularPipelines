using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;
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
        ModuleScheduler? scheduler = null;

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
                await StartModuleLegacy(nonParallelModule, false, null);
            }

            var keyedNonParallelModules = nonParallelModules
                .Where(x => x.GetType().GetCustomAttribute<NotInParallelAttribute>()!.ConstraintKeys.Length != 0)
                .ToList();

            await ProcessKeyedNonParallelModules(keyedNonParallelModules.ToList());

            var parallelModules = modules.Except(nonParallelModules).ToList();

            if (!parallelModules.Any())
            {
                return modules;
            }

            _logger.LogDebug("Initializing eager scheduler for {Count} parallel modules", parallelModules.Count);

            scheduler = new ModuleScheduler(_logger);
            scheduler.InitializeModules(parallelModules);

            var cancellationTokenSource = new CancellationTokenSource();
            var schedulerTask = scheduler.RunSchedulerAsync(cancellationTokenSource.Token);

            var maxDegreeOfParallelism = _parallelLimitProvider.GetLock(null)
                .GetType()
                .GetProperty("MaxConcurrentTasks")
                ?.GetValue(_parallelLimitProvider.GetLock(null)) as int? ?? Environment.ProcessorCount;

            _logger.LogDebug("Starting worker pool with MaxDegreeOfParallelism = {MaxDegreeOfParallelism}", maxDegreeOfParallelism);

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cancellationTokenSource.Token
            };

            try
            {
                await Parallel.ForEachAsync(
                    scheduler.ReadyModules.ReadAllAsync(cancellationTokenSource.Token),
                    parallelOptions,
                    async (moduleState, ct) =>
                    {
                        await ExecuteModuleWithScheduler(moduleState, scheduler, ct);
                    });
            }
            catch (Exception) when (_pipelineOptions.Value.ExecutionMode != ExecutionMode.StopOnFirstException)
            {
            }

            await schedulerTask;

            _logger.LogDebug("All parallel modules completed");

            if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
            {
                var failedModule = parallelModules.FirstOrDefault(m => m.Status == Enums.Status.Failed);
                if (failedModule != null)
                {
                    throw new AggregateException($"Module {failedModule.GetType().Name} failed in StopOnFirstException mode");
                }
            }

            return modules;
        }
        catch
        {
            foreach (var moduleBase in modules.Where(x => x.ModuleRunType == ModuleRunType.AlwaysRun))
            {
                var moduleTask = StartModuleLegacy(moduleBase, false, scheduler);
                try
                {
                    await moduleTask;
                }
                catch
                {
                    _ = moduleTask.Exception;
                }
            }

            throw;
        }
        finally
        {
            scheduler?.Dispose();
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
                    await StartModuleLegacy(module, false, null);
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

    private Task<ModuleBase> StartModuleLegacy(ModuleBase module, bool isStartedAsDependencyForOtherModule, ModuleScheduler? scheduler)
    {
        var task = module.GetOrStartExecutionTask(async () =>
            {
                using var semaphoreHandle = await WaitForParallelLimiter(module, isStartedAsDependencyForOtherModule);

                var moduleName = MarkupFormatter.FormatModuleName(module.GetType().Name);
                _logger.LogDebug("{Icon} Starting module {ModuleName}", MarkupFormatter.PlayIcon, moduleName);

                var dependencies = module.GetModuleDependencies();

                foreach (var dependency in dependencies.Reverse())
                {
                    await StartDependencyLegacy(module, dependency.DependencyType, dependency.IgnoreIfNotRegistered, scheduler);
                }

                try
                {
                    ModuleLogger.Values.Value = module.Context.Logger;

                    await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

                    var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(module.GetType());

                    await _mediator.Publish(new ModuleStartedNotification(module, estimatedDuration));

                    await module.StartInternal();

                    if (module.Status == Enums.Status.Skipped)
                    {
                        await _mediator.Publish(new ModuleSkippedNotification(module, module.SkipResult));
                        return;
                    }

                    await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

                    await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

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

    private async Task ExecuteModuleWithScheduler(ModuleState moduleState, ModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleName = MarkupFormatter.FormatModuleName(module.GetType().Name);

        try
        {
            scheduler.MarkModuleStarted(module.GetType());

            _logger.LogDebug("{Icon} Starting module {ModuleName}", MarkupFormatter.PlayIcon, moduleName);

            var dependencies = module.GetModuleDependencies();

            foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
            {
                var dependencyTask = scheduler.GetModuleCompletionTask(dependencyType);

                if (dependencyTask != null)
                {
                    try
                    {
                        await dependencyTask;
                    }
                    catch (Exception e) when (module.ModuleRunType == ModuleRunType.AlwaysRun)
                    {
                        _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                            $"{dependencyType.Name} threw an exception when {module.GetType().Name} was waiting for it as a dependency",
                            e));
                        module.Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
                    }
                }
                else if (!ignoreIfNotRegistered)
                {
                    throw new ModuleNotRegisteredException($"The module {dependencyType.Name} has not been registered", null);
                }
            }

            await module.GetOrStartExecutionTask(async () =>
            {
                using var semaphoreHandle = await WaitForParallelLimiter(module, false);

                try
                {
                    ModuleLogger.Values.Value = module.Context.Logger;

                    await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

                    var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(module.GetType());

                    await _mediator.Publish(new ModuleStartedNotification(module, estimatedDuration));

                    await module.StartInternal();

                    if (module.Status == Enums.Status.Skipped)
                    {
                        await _mediator.Publish(new ModuleSkippedNotification(module, module.SkipResult));
                        return;
                    }

                    await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

                    await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

                    var isSuccessful = module.Status == Enums.Status.Successful;
                    await _mediator.Publish(new ModuleCompletedNotification(module, isSuccessful));
                }
                finally
                {
                    if (!_pipelineOptions.Value.ShowProgressInConsole)
                    {
                        await _moduleDisposer.DisposeAsync(module);
                    }
                }
            });

            scheduler.MarkModuleCompleted(module.GetType(), true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Module {ModuleName} failed", moduleName);
            scheduler.MarkModuleCompleted(module.GetType(), false, ex);

            if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
            {
                throw;
            }
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

    private async Task StartDependencyLegacy(ModuleBase requestingModule, Type dependencyType, bool ignoreIfNotRegistered, ModuleScheduler? scheduler)
    {
        var dependencyName = MarkupFormatter.FormatModuleName(dependencyType.Name);
        var requestingModuleName = MarkupFormatter.FormatModuleName(requestingModule.GetType().Name);

        _logger.LogDebug("Starting dependency {Dependency} for module {Module}", dependencyName, requestingModuleName);

        if (scheduler != null)
        {
            var dependencyTask = scheduler.GetModuleCompletionTask(dependencyType);
            if (dependencyTask != null)
            {
                requestingModule.Context.Logger.LogDebug("{RequestingModule} is waiting for {Module}", requestingModuleName, dependencyName);
                try
                {
                    await dependencyTask;
                }
                catch (Exception e) when (requestingModule.ModuleRunType == ModuleRunType.AlwaysRun)
                {
                    _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                        $"{dependencyType.Name} threw an exception when {requestingModule.GetType().Name} was waiting for it as a dependency",
                        e));
                    requestingModule.Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
                }
                catch (DependencyFailedException e)
                {
                    _secondaryExceptionContainer.RegisterException(e);
                }
                catch (PipelineCancelledException e)
                {
                    _secondaryExceptionContainer.RegisterException(e);
                }
                return;
            }
        }

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

        var moduleTask = StartModuleLegacy(module, true, scheduler);

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
