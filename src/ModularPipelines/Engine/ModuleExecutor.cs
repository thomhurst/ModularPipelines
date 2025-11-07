using System.Reflection;
using System.Threading.Channels;
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
    private readonly IModuleSchedulerFactory _schedulerFactory;

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer,
        IEnumerable<ModuleBase> allModules,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IParallelLimitProvider parallelLimitProvider,
        IMediator mediator,
        ILogger<ModuleExecutor> logger,
        IModuleSchedulerFactory schedulerFactory)
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
        _schedulerFactory = schedulerFactory;
    }

    /// <summary>
    /// Executes a collection of modules using eager parallel scheduling.
    /// </summary>
    /// <param name="modules">The modules to execute. Must not be null.</param>
    /// <returns>The same collection of modules after execution.</returns>
    /// <remarks>
    /// This method:
    /// - Initializes a scheduler for managing module dependencies
    /// - Runs modules in parallel respecting dependencies and constraints
    /// - Handles exceptions based on configured ExecutionMode
    /// - Ensures AlwaysRun modules complete even on failure
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when modules is null.</exception>
    /// <exception cref="ModuleNotRegisteredException">Thrown when a module dependency is not registered.</exception>
    public async Task<IEnumerable<ModuleBase>> ExecuteAsync(IReadOnlyList<ModuleBase> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        if (modules.Count == 0)
        {
            _logger.LogDebug("No modules to execute");
            return modules;
        }

        IModuleScheduler? scheduler = null;

        try
        {
            scheduler = InitializeScheduler(modules);
            return await ExecuteWithSchedulerAsync(modules, scheduler);
        }
        catch (Exception outerEx)
        {
            _logger.LogDebug("Outer catch block entered with exception: {ExceptionType}", outerEx.GetType().Name);

            if (scheduler != null)
            {
                await WaitForAlwaysRunModulesAsync(scheduler, modules);
            }

            _logger.LogDebug("Outer catch block rethrowing exception");
            throw;
        }
        finally
        {
            scheduler?.Dispose();
        }
    }

    private IModuleScheduler InitializeScheduler(IReadOnlyList<ModuleBase> modules)
    {
        _logger.LogDebug("Initializing unified scheduler for {Count} modules", modules.Count);

        var scheduler = _schedulerFactory.Create();
        scheduler.InitializeModules(modules);

        return scheduler;
    }

    private async Task<IEnumerable<ModuleBase>> ExecuteWithSchedulerAsync(
        IReadOnlyList<ModuleBase> modules,
        IModuleScheduler scheduler)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        RegisterCancellationCallback(cancellationTokenSource, scheduler);

        var schedulerTask = scheduler.RunSchedulerAsync(cancellationTokenSource.Token);

        Exception? firstException;

        try
        {
            firstException = await ExecuteWorkerPoolAsync(scheduler, cancellationTokenSource);
        }
        finally
        {
            EnsureCancellation(cancellationTokenSource);
        }

        await schedulerTask;

        _logger.LogDebug("All modules completed");

        RethrowFirstExceptionIfPresent(firstException);

        _logger.LogDebug("ExecuteAsync returning normally with {Count} modules", modules.Count);
        return modules;
    }

    private void RegisterCancellationCallback(CancellationTokenSource cancellationTokenSource, IModuleScheduler scheduler)
    {
        // Register callback to cancel pending modules when cancellation is triggered
        // This ensures TaskCompletionSources for queued modules are properly completed
        cancellationTokenSource.Token.Register(() =>
        {
            _logger.LogDebug("Cancellation triggered - cancelling all pending modules");
            scheduler.CancelPendingModules();
        });
    }

    private async Task<Exception?> ExecuteWorkerPoolAsync(
        IModuleScheduler scheduler,
        CancellationTokenSource cancellationTokenSource)
    {
        var maxDegreeOfParallelism = _parallelLimitProvider.GetMaxDegreeOfParallelism();

        _logger.LogDebug("Starting worker pool with MaxDegreeOfParallelism = {MaxDegreeOfParallelism}",
            maxDegreeOfParallelism);

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism,
            CancellationToken = cancellationTokenSource.Token
        };

        Exception? firstException = null;

        try
        {
            await Parallel.ForEachAsync(
                scheduler.ReadyModules.ReadAllAsync(cancellationTokenSource.Token),
                parallelOptions,
                async (moduleState, ct) =>
                {
                    try
                    {
                        await ExecuteModule(moduleState, scheduler, ct);
                    }
                    catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
                    {
                        // Store first exception and cancel immediately to stop scheduler and other workers
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        cancellationTokenSource.Cancel();
                        // Don't rethrow here - let parallel loop complete and rethrow after cleanup
                    }
                });
        }
        catch (OperationCanceledException) when (firstException != null)
        {
            // Expected when we cancelled due to StopOnFirstException - will rethrow firstException below
        }
        catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode != ExecutionMode.StopOnFirstException)
        {
            _logger.LogDebug(ex, "Module execution failed but continuing due to ExecutionMode.WaitForAllModules");
        }

        return firstException;
    }

    private void EnsureCancellation(CancellationTokenSource cancellationTokenSource)
    {
        // Cancel scheduler when workers exit if not already cancelled
        // This handles the normal completion case
        if (!cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
    }

    private void RethrowFirstExceptionIfPresent(Exception? firstException)
    {
        // If we stored an exception during StopOnFirstException, rethrow it to preserve the original exception type
        if (firstException != null)
        {
            _logger.LogDebug("Rethrowing first exception: {ExceptionType}", firstException.GetType().Name);
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(firstException).Throw();
        }
    }

    private async Task WaitForAlwaysRunModulesAsync(IModuleScheduler scheduler, IReadOnlyList<ModuleBase> modules)
    {
        var alwaysRunModules = modules.Where(x => x.ModuleRunType == ModuleRunType.AlwaysRun).ToList();
        _logger.LogDebug("Found {Count} AlwaysRun modules", alwaysRunModules.Count);

        foreach (var moduleBase in alwaysRunModules)
        {
            await WaitForSingleAlwaysRunModuleAsync(scheduler, moduleBase);
        }
    }

    private async Task WaitForSingleAlwaysRunModuleAsync(IModuleScheduler scheduler, ModuleBase moduleBase)
    {
        var moduleState = scheduler.GetModuleState(moduleBase.GetType());
        var moduleTask = scheduler.GetModuleCompletionTask(moduleBase.GetType());

        if (moduleTask == null || moduleState == null)
        {
            return;
        }

        // Only wait for modules that have actually started executing or already completed
        // Skip modules that are still pending/queued as they will never complete after cancellation
        if (ShouldWaitForAlwaysRunModule(moduleState))
        {
            _logger.LogDebug("Awaiting AlwaysRun module: {ModuleName} (IsExecuting={IsExecuting}, IsCompleted={IsCompleted})",
                moduleBase.GetType().Name, moduleState.IsExecuting, moduleState.IsCompleted);

            try
            {
                await moduleTask;
                _logger.LogDebug("AlwaysRun module {ModuleName} completed", moduleBase.GetType().Name);
            }
            catch (Exception alwaysRunEx)
            {
                _logger.LogDebug("AlwaysRun module {ModuleName} threw: {ExceptionType}",
                    moduleBase.GetType().Name, alwaysRunEx.GetType().Name);
                _ = moduleTask.Exception; // Observe exception
            }
        }
        else
        {
            _logger.LogDebug("Skipping AlwaysRun module {ModuleName} as it never started (IsQueued={IsQueued}, IsExecuting={IsExecuting}, IsCompleted={IsCompleted})",
                moduleBase.GetType().Name, moduleState.IsQueued, moduleState.IsExecuting, moduleState.IsCompleted);
        }
    }

    private static bool ShouldWaitForAlwaysRunModule(ModuleState moduleState)
    {
        return moduleState.IsExecuting || moduleState.IsCompleted;
    }

    private async Task ExecuteModule(ModuleState moduleState, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleName = MarkupFormatter.FormatModuleName(module.GetType().Name);

        try
        {
            scheduler.MarkModuleStarted(module.GetType());

            _logger.LogDebug("{Icon} Starting module {ModuleName}", MarkupFormatter.PlayIcon, moduleName);

            await WaitForDependenciesAsync(module, scheduler);

            await module.GetOrStartExecutionTask(() => ExecuteModuleTaskAsync(module));

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

    private async Task ExecuteModuleCore(ModuleBase module)
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

    private async Task ExecuteModuleTaskAsync(ModuleBase module)
    {
        using var semaphoreHandle = await WaitForParallelLimiter(module);

        try
        {
            await ExecuteModuleCore(module);
        }
        finally
        {
            if (!_pipelineOptions.Value.ShowProgressInConsole)
            {
                await _moduleDisposer.DisposeAsync(module);
            }
        }
    }

    private async Task<IDisposable> WaitForParallelLimiter(ModuleBase module)
    {
        var parallelLimitAttributeType =
            module.GetType().GetCustomAttributes<ParallelLimiterAttribute>().FirstOrDefault()?.Type;

        if (parallelLimitAttributeType != null)
        {
            return await _parallelLimitProvider.GetLock(parallelLimitAttributeType).WaitAsync();
        }

        return NoOpDisposable.Instance;
    }

    private async Task WaitForDependenciesAsync(ModuleBase module, IModuleScheduler scheduler)
    {
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
                var message = $"Module '{module.GetType().Name}' depends on '{dependencyType.Name}', " +
                              $"but '{dependencyType.Name}' has not been registered in the pipeline.\n\n" +
                              $"Suggestions:\n" +
                              $"  1. Add '.AddModule<{dependencyType.Name}>()' to your pipeline configuration before '.AddModule<{module.GetType().Name}>()'\n" +
                              $"  2. Use '[DependsOn<{dependencyType.Name}>(ignoreIfNotRegistered: true)]' if this dependency is optional\n" +
                              $"  3. Check for typos in the dependency type name\n" +
                              $"  4. Verify that '{dependencyType.Name}' is in a project referenced by your pipeline project";
                throw new ModuleNotRegisteredException(message, null);
            }
        }
    }
}
