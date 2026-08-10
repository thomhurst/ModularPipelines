using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Exceptions;
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
/// - <see cref="IModuleRunner"/>: Executes individual modules.
/// - <see cref="IAlwaysRunHandler"/>: Handles AlwaysRun module completion.
/// - <see cref="IModuleResultRegistrar"/>: Registers module results.
/// </remarks>
internal class ModuleExecutor : IModuleExecutor
{
    private readonly IModuleSchedulerFactory _schedulerFactory;
    private readonly IModuleRunner _moduleRunner;
    private readonly IAlwaysRunHandler _alwaysRunHandler;
    private readonly IModuleResultRegistrar _resultRegistrar;
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly IRegistrationEventExecutor _registrationEventExecutor;
    private readonly IMetricsCollector _metricsCollector;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ILogger<ModuleExecutor> _logger;

    public ModuleExecutor(
        IModuleSchedulerFactory schedulerFactory,
        IModuleRunner moduleRunner,
        IAlwaysRunHandler alwaysRunHandler,
        IModuleResultRegistrar resultRegistrar,
        IModuleResultRegistry resultRegistry,
        IParallelLimitProvider parallelLimitProvider,
        IRegistrationEventExecutor registrationEventExecutor,
        IMetricsCollector metricsCollector,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IOptions<PipelineOptions> pipelineOptions,
        ILogger<ModuleExecutor> logger)
    {
        _schedulerFactory = schedulerFactory;
        _moduleRunner = moduleRunner;
        _alwaysRunHandler = alwaysRunHandler;
        _resultRegistrar = resultRegistrar;
        _resultRegistry = resultRegistry;
        _parallelLimitProvider = parallelLimitProvider;
        _registrationEventExecutor = registrationEventExecutor;
        _metricsCollector = metricsCollector;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _secondaryExceptionContainer = secondaryExceptionContainer;
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
                try
                {
                    await _alwaysRunHandler.WaitForAlwaysRunModulesAsync(scheduler, modules).ConfigureAwait(false);
                }
                catch (Exception alwaysRunException)
                {
                    var pipelineExceptions = FlattenDistinctExceptions(outerEx);
                    var combinedExceptions = FlattenDistinctExceptions(
                        outerEx,
                        alwaysRunException);
                    if (combinedExceptions.Count == pipelineExceptions.Count)
                    {
                        ExceptionDispatchInfo.Capture(outerEx).Throw();
                    }

                    throw new AggregateException(
                        "Pipeline execution and AlwaysRun teardown both failed.",
                        combinedExceptions);
                }
            }

            _logger.LogDebug("Outer catch block rethrowing exception");
            throw;
        }
        finally
        {
            scheduler?.Dispose();
        }
    }

    internal static IReadOnlyList<Exception> FlattenDistinctExceptions(
        params Exception[] exceptions) =>
        exceptions
            .SelectMany(FlattenException)
            .Distinct<Exception>(ReferenceEqualityComparer.Instance)
            .ToArray();

    private static IEnumerable<Exception> FlattenException(Exception exception) =>
        exception is AggregateException { InnerExceptions.Count: > 0 } aggregateException
            ? aggregateException.InnerExceptions.SelectMany(FlattenException)
            : [exception];

    private async Task<IModuleScheduler> InitializeSchedulerAsync(IReadOnlyList<IModule> modules)
    {
        _logger.LogDebug("Initializing unified scheduler for {Count} modules", modules.Count);

        // Record pipeline start time for metrics
        _metricsCollector.SetPipelineStartTime(DateTimeOffset.UtcNow);

        // Invoke registration events before dependency resolution
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(modules).ConfigureAwait(false);

        // Revalidate the runnable set now that registration-event dependencies are populated and
        // discovery has settled. Build-time validation only saw the statically registered graph;
        // dependencies added via IModuleRegistrationContext.AddDependency, and modules whose
        // runnability depends on stateful run conditions, are only fully known here. Validating
        // the runnable set against itself mirrors the DependencyWaiter check (a required
        // dependency outside the runnable set throws), surfacing missing/cyclic dependencies
        // eagerly with a clear message instead of failing later in the scheduler.
        ModuleDependencyValidator.Validate(
            modules,
            _dependencyRegistry,
            _metadataRegistry,
            UsedHistoryModuleSchedulerInitializer.GetPrecompletedModuleTypes(modules, _resultRegistry));

        var scheduler = _schedulerFactory.Create();
        scheduler.InitializeModules(modules);
        UsedHistoryModuleSchedulerInitializer.Precomplete(
            modules,
            scheduler,
            _resultRegistry);

        return scheduler;
    }

    private async Task<IEnumerable<IModule>> ExecuteWithSchedulerAsync(
        IReadOnlyList<IModule> modules,
        IModuleScheduler scheduler)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        RegisterCancellationCallback(cancellationTokenSource, scheduler);

        var schedulerTask = scheduler.RunSchedulerAsync(cancellationTokenSource.Token);

        WorkerFailure? firstFailure;

        try
        {
            firstFailure = await ExecuteWorkerPoolAsync(scheduler, cancellationTokenSource).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            var cancelledModules =
                scheduler.CancelPendingModules();
            RegisterCancelledModuleResults(
                scheduler,
                cancelledModules,
                exception);
            throw;
        }
        finally
        {
            EnsureCancellation(cancellationTokenSource);
            TaskObservation.ObserveFault(schedulerTask);
        }

        try
        {
            await schedulerTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (firstFailure != null)
        {
            // Expected when cancellation was triggered due to module failure.
            // The scheduler throws OperationCanceledException from WaitForNextSchedulingOpportunity
            // when the cancellation token is cancelled.
            _logger.LogDebug("Scheduler cancelled due to module failure");
        }

        _logger.LogDebug("All modules completed");

        // Register PipelineTerminated results for modules that were cancelled before they started
        if (firstFailure != null)
        {
            RegisterCancelledModuleResults(
                scheduler,
                modules,
                firstFailure.Exception,
                firstFailure.ModuleType);
        }

        ThrowWorkerExceptionsIfPresent(firstFailure?.Exception);

        _logger.LogDebug("ExecuteAsync returning normally with {Count} modules", modules.Count);
        return modules;
    }

    private void RegisterCancellationCallback(CancellationTokenSource cancellationTokenSource, IModuleScheduler scheduler)
    {
        cancellationTokenSource.Token.Register(
            () => scheduler.CancelPendingModules());
    }

    private async Task<WorkerFailure?> ExecuteWorkerPoolAsync(
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

        WorkerFailure? firstFailure = null;
        var recordedWorkerExceptions =
            new ConcurrentDictionary<Exception, byte>(ReferenceEqualityComparer.Instance);

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
                        var failedModuleType = FindFailedModuleType(ex) ?? moduleState.ModuleType;
                        var pipelineException = GetPipelineException(ex);
                        var isFirstFailure = false;

                        if (!IsExpectedWorkerCancellation(ex, ct)
                            && recordedWorkerExceptions.TryAdd(pipelineException, 0))
                        {
                            _secondaryExceptionContainer.RegisterException(pipelineException);
                            var workerFailure = new WorkerFailure(
                                pipelineException,
                                failedModuleType);
                            isFirstFailure = Interlocked.CompareExchange(
                                ref firstFailure,
                                workerFailure,
                                null) == null;
                        }

                        if (isFirstFailure)
                        {
                            var cancelledModules =
                                scheduler.CancelPendingModules();
                            RegisterCancelledModuleResults(
                                scheduler,
                                cancelledModules,
                                pipelineException,
                                firstFailure!.ModuleType);
                        }

                        EnsureCancellation(cancellationTokenSource);
                    }
                    catch (Exception ex)
                    {
                        HandleWaitForAllWorkerFailure(moduleState, scheduler, ex);
                    }
                }).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (firstFailure != null)
        {
            // Expected when we cancelled due to StopOnFirstException
        }

        return firstFailure;
    }

    // Engine-linked module cancellation is converted to a PipelineTerminated result
    // by ModuleExecutionPipeline. Only worker-token cancellation can reach this layer.
    internal static bool IsExpectedWorkerCancellation(
        Exception exception,
        CancellationToken workerCancellationToken)
    {
        return exception is OperationCanceledException operationCanceledException
               && workerCancellationToken.IsCancellationRequested
               && operationCanceledException.CancellationToken == workerCancellationToken;
    }

    private static Exception GetPipelineException(Exception exception)
    {
        while (exception is DependencyFailedException { InnerException: { } innerException })
        {
            exception = innerException;
        }

        return exception;
    }

    private void RegisterCancelledModuleResults(
        IModuleScheduler scheduler,
        IReadOnlyList<IModule> modules,
        Exception exception,
        Type? failedModuleType = null)
    {
        failedModuleType = FindFailedModuleType(exception) ?? failedModuleType;
        if (failedModuleType is null
            || scheduler.GetModuleState(failedModuleType)?.Module is not { } failedModule)
        {
            _resultRegistrar.RegisterTerminatedResultsForCancelledModules(modules, exception);
            return;
        }

        foreach (var module in modules)
        {
            if (module.Configuration.AlwaysRun
                || _resultRegistry.GetResult(module.GetType()) is not null)
            {
                continue;
            }

            var moduleType = module.GetType();
            if (DependsOnFailedModule(scheduler, moduleType, failedModuleType))
            {
                _resultRegistrar.RegisterDependencyFailedResult(
                    module,
                    moduleType,
                    new DependencyFailedException(exception, failedModule));
            }
            else
            {
                _resultRegistrar.RegisterTerminatedResult(module, moduleType, exception);
            }
        }
    }

    private static Type? FindFailedModuleType(Exception exception) =>
        exception switch
        {
            ModuleFailedException moduleFailure => moduleFailure.ModuleType,
            DependencyFailedException dependencyFailure => dependencyFailure.FailingModuleType,
            AggregateException aggregateException => aggregateException.InnerExceptions
                .Select(FindFailedModuleType)
                .FirstOrDefault(moduleType => moduleType is not null),
            { InnerException: { } innerException } => FindFailedModuleType(innerException),
            _ => null,
        };

    private static bool DependsOnFailedModule(
        IModuleScheduler scheduler,
        Type moduleType,
        Type failedModuleType)
    {
        var visitedModules = new HashSet<Type>();
        return HasDependencyPath(moduleType);

        bool HasDependencyPath(Type candidateType)
        {
            if (!visitedModules.Add(candidateType)
                || scheduler.GetModuleState(candidateType) is not { } moduleState)
            {
                return false;
            }

            if (moduleState.Module.Configuration.AlwaysRun)
            {
                return false;
            }

            return moduleState.Dependencies.Keys.Any(dependencyType =>
                dependencyType == failedModuleType
                || HasDependencyPath(dependencyType));
        }
    }

    private void HandleWaitForAllWorkerFailure(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        Exception exception)
    {
        _secondaryExceptionContainer.RegisterException(exception);
        TryMarkModuleFailed(moduleState, scheduler, exception);
        TryRegisterTerminatedResult(moduleState, exception);

        _logger.LogError(
            exception,
            "Module worker failed but remaining modules will continue due to ExecutionMode.WaitForAllModules");
    }

    private void TryMarkModuleFailed(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        Exception exception)
    {
        try
        {
            scheduler.MarkModuleCompleted(moduleState.ModuleType, success: false, exception);
        }
        catch (Exception recoveryException)
        {
            _secondaryExceptionContainer.RegisterException(recoveryException);
            _logger.LogError(
                recoveryException,
                "Failed to complete scheduler state for faulted module {ModuleName}",
                moduleState.ModuleType.Name);
        }
    }

    private void TryRegisterTerminatedResult(ModuleState moduleState, Exception exception)
    {
        try
        {
            if (_resultRegistry.GetResult(moduleState.ModuleType) == null)
            {
                _resultRegistrar.RegisterTerminatedResult(
                    moduleState.Module,
                    moduleState.ModuleType,
                    exception);
            }
        }
        catch (Exception recoveryException)
        {
            _secondaryExceptionContainer.RegisterException(recoveryException);
            _logger.LogError(
                recoveryException,
                "Failed to register terminated result for faulted module {ModuleName}",
                moduleState.ModuleType.Name);
        }
    }

    private void EnsureCancellation(CancellationTokenSource cancellationTokenSource)
    {
        if (!cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
    }

    private void ThrowWorkerExceptionsIfPresent(Exception? firstException)
    {
        if (firstException != null)
        {
            _logger.LogDebug("Throwing worker exceptions after first failure: {ExceptionType}", firstException.GetType().Name);
            _secondaryExceptionContainer.ThrowExceptions();

            // Defensive fallback for custom container implementations that do not throw.
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(firstException).Throw();
        }
    }

    private sealed record WorkerFailure(Exception Exception, Type ModuleType);
}
