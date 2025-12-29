using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Orchestrates the execution of a collection of modules using eager parallel scheduling.
/// Delegates to specialized components for individual responsibilities.
/// </summary>
/// <remarks>
/// This class follows the Single Responsibility Principle by focusing solely on orchestration.
/// Individual responsibilities are delegated to:
/// - <see cref="IModuleRunner"/>: Executes individual modules
/// - <see cref="IAlwaysRunHandler"/>: Handles AlwaysRun module completion
/// - <see cref="IModuleResultRegistrar"/>: Registers module results
/// </remarks>
internal class ModuleExecutor : IModuleExecutor
{
    private readonly IModuleSchedulerFactory _schedulerFactory;
    private readonly IModuleRunner _moduleRunner;
    private readonly IAlwaysRunHandler _alwaysRunHandler;
    private readonly IModuleResultRegistrar _resultRegistrar;
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly IRegistrationEventExecutor _registrationEventExecutor;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ILogger<ModuleExecutor> _logger;

    public ModuleExecutor(
        IModuleSchedulerFactory schedulerFactory,
        IModuleRunner moduleRunner,
        IAlwaysRunHandler alwaysRunHandler,
        IModuleResultRegistrar resultRegistrar,
        IParallelLimitProvider parallelLimitProvider,
        IRegistrationEventExecutor registrationEventExecutor,
        IMetricsCollector metricsCollector,
        IOptions<PipelineOptions> pipelineOptions,
        ILogger<ModuleExecutor> logger)
    {
        _schedulerFactory = schedulerFactory;
        _moduleRunner = moduleRunner;
        _alwaysRunHandler = alwaysRunHandler;
        _resultRegistrar = resultRegistrar;
        _parallelLimitProvider = parallelLimitProvider;
        _registrationEventExecutor = registrationEventExecutor;
        _metricsCollector = metricsCollector;
        _pipelineOptions = pipelineOptions;
        _logger = logger;
    }

    /// <summary>
    /// Executes a collection of modules using eager parallel scheduling.
    /// </summary>
    public async Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules)
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
            scheduler = await InitializeSchedulerAsync(modules).ConfigureAwait(false);
            return await ExecuteWithSchedulerAsync(modules, scheduler).ConfigureAwait(false);
        }
        catch (Exception outerEx)
        {
            _logger.LogDebug("Outer catch block entered with exception: {ExceptionType}", outerEx.GetType().Name);

            if (scheduler != null)
            {
                await _alwaysRunHandler.WaitForAlwaysRunModulesAsync(scheduler, modules).ConfigureAwait(false);
            }

            _logger.LogDebug("Outer catch block rethrowing exception");
            throw;
        }
        finally
        {
            scheduler?.Dispose();
        }
    }

    private async Task<IModuleScheduler> InitializeSchedulerAsync(IReadOnlyList<IModule> modules)
    {
        _logger.LogDebug("Initializing unified scheduler for {Count} modules", modules.Count);

        // Record pipeline start time for metrics
        _metricsCollector.SetPipelineStartTime(DateTimeOffset.UtcNow);

        // Invoke registration events before dependency resolution
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(modules).ConfigureAwait(false);

        var scheduler = _schedulerFactory.Create();
        scheduler.InitializeModules(modules);

        return scheduler;
    }

    private async Task<IEnumerable<IModule>> ExecuteWithSchedulerAsync(
        IReadOnlyList<IModule> modules,
        IModuleScheduler scheduler)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        RegisterCancellationCallback(cancellationTokenSource, scheduler);

        var schedulerTask = scheduler.RunSchedulerAsync(cancellationTokenSource.Token);

        Exception? firstException;

        try
        {
            firstException = await ExecuteWorkerPoolAsync(scheduler, cancellationTokenSource).ConfigureAwait(false);
        }
        finally
        {
            EnsureCancellation(cancellationTokenSource);
        }

        await schedulerTask.ConfigureAwait(false);

        _logger.LogDebug("All modules completed");

        // Register PipelineTerminated results for modules that were cancelled before they started
        if (firstException != null)
        {
            _resultRegistrar.RegisterTerminatedResultsForCancelledModules(modules, firstException);
        }

        RethrowFirstExceptionIfPresent(firstException);

        _logger.LogDebug("ExecuteAsync returning normally with {Count} modules", modules.Count);
        return modules;
    }

    private void RegisterCancellationCallback(CancellationTokenSource cancellationTokenSource, IModuleScheduler scheduler)
    {
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
            CancellationToken = cancellationTokenSource.Token,
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
                        await _moduleRunner.ExecuteAsync(moduleState, scheduler, ct).ConfigureAwait(false);
                    }
                    catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
                    {
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        cancellationTokenSource.Cancel();
                    }
                }).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (firstException != null)
        {
            // Expected when we cancelled due to StopOnFirstException
        }
        catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode != ExecutionMode.StopOnFirstException)
        {
            _logger.LogDebug(ex, "Module execution failed but continuing due to ExecutionMode.WaitForAllModules");
        }

        return firstException;
    }

    private void EnsureCancellation(CancellationTokenSource cancellationTokenSource)
    {
        if (!cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
    }

    private void RethrowFirstExceptionIfPresent(Exception? firstException)
    {
        if (firstException != null)
        {
            _logger.LogDebug("Rethrowing first exception: {ExceptionType}", firstException.GetType().Name);
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(firstException).Throw();
        }
    }
}
