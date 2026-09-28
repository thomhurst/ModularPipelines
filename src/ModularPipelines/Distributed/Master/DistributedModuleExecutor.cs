using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Distributed.Master;

internal class DistributedModuleExecutor(
    IHostApplicationLifetime lifetime,
    IModuleSchedulerFactory schedulerFactory,
    IModuleRunner moduleRunner,
    IAlwaysRunHandler alwaysRunHandler,
    IRegistrationEventExecutor registrationEventExecutor,
    IDistributedMasterCoordinator masterCoordinator,
    IDistributedWorkerCoordinator workerCoordinator,
    DistributedWorkPublisher publisher,
    DistributedResultCollector resultCollector,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleResultRegistry resultRegistry,
    IModuleResultRegistrar resultRegistrar,
    IModuleDependencyRegistry dependencyRegistry,
    IModuleMetadataRegistry metadataRegistry,
    IOptions<DistributedOptions> options,
    IParallelLimitProvider parallelLimitProvider,
    IServiceScopeFactory serviceScopeFactory,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<DistributedModuleExecutor> logger,
    IModuleCacheResultRepository? cacheResultRepository = null,
    IOptions<PipelineOptions>? pipelineOptions = null,
    DistributedCacheHitTracker? cacheHitTracker = null,
    IEnumerable<IModule>? registeredModules = null,
    LocalCapabilityRegistry? localCapabilities = null,
    IMetricsCollector? metricsCollector = null,
    AcceptedArtifactRegistry? acceptedArtifacts = null,
    IExecutionLocationContext? executionLocationContext = null) : IExecutionBackend
{
    private readonly IReadOnlyList<IModule> _registeredModules = registeredModules?.ToArray() ?? [];

    private static readonly TimeSpan WorkerRegistrationPollInterval = TimeSpan.FromMilliseconds(250);

    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IModuleSchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly IAlwaysRunHandler _alwaysRunHandler = alwaysRunHandler;
    private readonly IRegistrationEventExecutor _registrationEventExecutor = registrationEventExecutor;
    private readonly IDistributedMasterCoordinator _masterCoordinator = masterCoordinator;
    private readonly IDistributedWorkerCoordinator _workerCoordinator = workerCoordinator;
    private readonly DistributedWorkPublisher _publisher = publisher;
    private readonly DistributedResultCollector _resultCollector = resultCollector;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleResultRegistrar _resultRegistrar = resultRegistrar;
    private readonly IModuleDependencyRegistry _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry = metadataRegistry;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly IParallelLimitProvider _parallelLimitProvider = parallelLimitProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ArtifactLifecycleManager? _artifactLifecycleManager = artifactLifecycleManager;
    private readonly ILogger<DistributedModuleExecutor> _logger = logger;
    private readonly IModuleCacheResultRepository? _cacheResultRepository = cacheResultRepository;
    private readonly IOptions<PipelineOptions>? _pipelineOptions = pipelineOptions;
    private readonly DistributedCacheHitTracker _cacheHitTracker = cacheHitTracker ?? new();
    private readonly IMetricsCollector? _metricsCollector = metricsCollector;
    private readonly AcceptedArtifactRegistry? _acceptedArtifacts = acceptedArtifacts;
    private readonly ConcurrentDictionary<ModuleId, ResultDeadline> _resultDeadlines = new();
    private readonly DistributedAssignmentExecutor _assignmentExecutor = new(
        workerCoordinator,
        typeRegistry,
        serializer,
        moduleRunner,
        resultRegistry,
        dependencyRegistry,
        metadataRegistry,
        serviceScopeFactory,
        artifactLifecycleManager,
        acceptedArtifacts,
        executionLocationContext,
        logger);

    public bool OwnsEntirePlan => true;

    private WorkerId MasterWorkerId => _options.Value.LocalWorkerId;

    public Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        ExecutionBackendRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return ExecuteAsync(
            request.Modules,
            request.GetEstimatedDurationsByType(),
            request.Context,
            cancellationToken);
    }

    internal async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
        IExecutionBackendContext context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(estimatedDurations);
        ArgumentNullException.ThrowIfNull(context);

        if (modules.Count == 0)
        {
            return [];
        }

        var workerMaxConcurrency = DistributedWorkerPool.GetMaxConcurrency(
            _parallelLimitProvider,
            _options.Value);

        // Compare the registered pipeline on every process, even when execution selects a subset.
        foreach (var module in _registeredModules.Concat(modules))
        {
            _typeRegistry.Register(module.GetType());
        }

        // Build O(1) lookup for module resolution
        var moduleLookup = DependencyResultApplicator.BuildModuleLookup(modules);

        // Invoke registration events before dependency resolution
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(modules).ConfigureAwait(false);

        // Revalidate the runnable set now that registration-event dependencies are populated, so
        // missing/self/cyclic dependencies (including ones added via AddDependency) fail fast here
        // rather than the master hanging or failing late. Mirrors the standalone ModuleExecutor.
        ModuleDependencyValidator.Validate(
            modules,
            _dependencyRegistry,
            _metadataRegistry,
            UsedHistoryModuleSchedulerInitializer.GetPrecompletedModuleTypes(modules, _resultRegistry));

        using var executionCts = CancellationTokenSource.CreateLinkedTokenSource(
            _lifetime.ApplicationStopping,
            cancellationToken);
        IModuleScheduler? scheduler = null;
        var failureBroadcast = new FailureBroadcast(this);
        try
        {
            // Wait for workers to register before distributing work. Keep this inside the
            // shutdown scope so cancellation or coordinator failure still notifies workers.
            var options = _options.Value;
            var registrationDeadline = DateTimeOffset.UtcNow + options.WorkerRegistrationTimeout;
            await WaitForMinimumWorkersAsync(registrationDeadline, executionCts.Token)
                .ConfigureAwait(false);
            var masterCapabilities = await LocalCapabilities.GetAsync(
                    localCapabilities,
                    options,
                    executionCts.Token)
                .ConfigureAwait(false);

            scheduler = _schedulerFactory.Create();
            scheduler.InitializeModules(modules, estimatedDurations);
            UsedHistoryModuleSchedulerInitializer.Precomplete(
                modules,
                scheduler,
                _resultRegistry);
            await PublishPrecompletedResultsAsync(modules, executionCts.Token)
                .ConfigureAwait(false);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(executionCts.Token);
            using var masterWorkerCts = CancellationTokenSource.CreateLinkedTokenSource(
                _lifetime.ApplicationStopping);
            var executionToken = cts.Token;
            using var cancellationRegistration = executionToken.Register(
                () => CompleteCancelledModules(scheduler, _resultRegistrar, executionToken));

            var schedulerTask = scheduler.RunSchedulerAsync(executionToken);

            // The master participates as a worker, claiming leases from the same queue as external
            // workers, and maintains leases for the whole run.
            var inFlightLeases = new InFlightLeases();
            var masterWorkerTask = RunMasterWorkerLoopAsync(
                modules,
                moduleLookup,
                masterCapabilities,
                workerMaxConcurrency,
                inFlightLeases,
                executionToken,
                masterWorkerCts.Token);
            var maintenanceTask = RunLeaseMaintenanceAsync(inFlightLeases, masterWorkerCts.Token);

            var plan = new DistributedPlan(scheduler, cts, context, failureBroadcast.Request, masterCapabilities, modules);
            var resultTasks = await PublishReadyModulesAsync(plan).ConfigureAwait(false);
            await IgnoreCancellationAsync(Task.WhenAll(resultTasks)).ConfigureAwait(false);
            await FinalizeExecutionAsync(
                    plan,
                    masterWorkerCts,
                    masterWorkerTask,
                    maintenanceTask,
                    schedulerTask)
                .ConfigureAwait(false);
        }
        catch
        {
            failureBroadcast.Request();
            throw;
        }
        finally
        {
            await SignalWorkerShutdownAsync(failureBroadcast).ConfigureAwait(false);
            scheduler?.Dispose();
            foreach (var deadline in _resultDeadlines.Values)
            {
                deadline.Dispose();
            }

            _resultDeadlines.Clear();
        }

        return _resultRegistry.GetCompletedResults(modules);
    }

    internal Task<IReadOnlyList<IModuleResult>> ExecuteAsync(IReadOnlyList<IModule> modules) =>
        ExecuteAsync(modules, new Dictionary<Type, TimeSpan>());

    internal Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations)
    {
        return ExecuteAsync(
            modules,
            estimatedDurations,
            new ExecutionBackendContext(_resultRegistry),
            CancellationToken.None);
    }

    private async Task PublishPrecompletedResultsAsync(
        IReadOnlyList<IModule> modules,
        CancellationToken cancellationToken)
    {
        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            var result = _resultRegistry.GetResult(moduleType);
            if (result?.Status != ModuleStatus.RestoredFromHistory)
            {
                continue;
            }

            var serialized = _serializer.Serialize(
                result,
                ModuleId.FromType(moduleType),
                MasterWorkerId);
            await _masterCoordinator.PublishResultAsync(serialized, lease: null, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task<IReadOnlyList<Task>> PublishReadyModulesAsync(DistributedPlan plan)
    {
        var resultTasks = new List<Task>();
        try
        {
            await foreach (var moduleState in plan.Scheduler.ReadyModules.ReadAllAsync(plan.PipelineCts.Token)
                               .ConfigureAwait(false))
            {
                resultTasks.Add(RestoreOrExecuteDistributedModuleAsync(moduleState, plan));
            }
        }
        catch (OperationCanceledException)
        {
            // Expected during shutdown.
        }

        return resultTasks;
    }

    private async Task RestoreOrExecuteDistributedModuleAsync(ModuleState moduleState, DistributedPlan plan)
    {
        plan.PipelineCts.Token.ThrowIfCancellationRequested();

        if (await TryRestoreCachedResultAsync(moduleState, plan.Scheduler, plan.Context, plan.PipelineCts.Token)
                .ConfigureAwait(false))
        {
            if (moduleState.Result?.ExceptionOrDefault is not null)
            {
                plan.RequestFailureCancellation();
                await plan.PipelineCts.CancelAsync().ConfigureAwait(false);
            }

            return;
        }

        var collectTask = await TryStartDistributedExecutionAsync(moduleState, plan).ConfigureAwait(false);
        if (collectTask is not null)
        {
            await collectTask.ConfigureAwait(false);
        }
    }

    private async Task FinalizeExecutionAsync(
        DistributedPlan plan,
        CancellationTokenSource masterWorkerCts,
        Task masterWorkerTask,
        Task maintenanceTask,
        Task schedulerTask)
    {
        Exception? alwaysRunException = null;
        try
        {
            await CompleteAlwaysRunModulesAsync(plan, masterWorkerCts).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            plan.RequestFailureCancellation();
            alwaysRunException = exception;
        }

        if (!plan.PipelineCts.IsCancellationRequested)
        {
            await plan.PipelineCts.CancelAsync().ConfigureAwait(false);
        }

        await IgnoreCancellationAsync(masterWorkerTask).ConfigureAwait(false);
        await IgnoreCancellationAsync(maintenanceTask).ConfigureAwait(false);
        await IgnoreCancellationAsync(schedulerTask).ConfigureAwait(false);

        if (alwaysRunException is not null)
        {
            ExceptionDispatchInfo.Capture(alwaysRunException).Throw();
        }
    }

    private static async Task IgnoreCancellationAsync(Task task)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected when pipeline or worker shutdown stops background work.
        }
    }

    private async Task SignalWorkerShutdownAsync(FailureBroadcast failureBroadcast)
    {
        // Always signal workers to stop, whether the master succeeded or crashed.
        // Without this, workers wait for work that will never come.
        if (_lifetime.ApplicationStopping.IsCancellationRequested)
        {
            await BroadcastCancellationAsync(DistributedCancellationReason.Stopped).ConfigureAwait(false);
        }

        await failureBroadcast.WaitAsync().ConfigureAwait(false);

        try
        {
            await _masterCoordinator.SignalCompletionAsync(CancellationToken.None).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to signal completion to workers during shutdown");
        }
    }

    private async Task BroadcastCancellationAsync(DistributedCancellationReason reason)
    {
        try
        {
            await _masterCoordinator.BroadcastCancellationAsync(reason, CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to broadcast {Reason} cancellation to workers", reason);
        }
    }

    private async Task<Task?> TryStartDistributedExecutionAsync(ModuleState moduleState, DistributedPlan plan)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var cleanupDeferredToResultTask = false;
        try
        {
            var assignment = await _publisher.CreateAssignmentAsync(
                    module,
                    plan.PipelineCts.Token,
                    moduleState.Priority,
                    moduleState.CriticalPathWeight,
                    plan.Modules)
                .ConfigureAwait(false);
            if (!plan.Scheduler.MarkModuleStarted(moduleType))
            {
                return null;
            }

            cleanupDeferredToResultTask = true;
            return PublishAndCollectDistributedResultAsync(assignment, module, moduleType, plan);
        }
        catch (OperationCanceledException) when (plan.PipelineCts.IsCancellationRequested)
        {
            return null;
        }
        catch (UnsatisfiableModuleRequirementException exception)
        {
            CompleteUnroutableModule(moduleState, plan.Scheduler, exception, plan.Context);
            return null;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to create a distributed assignment for module {Module}",
                moduleType.Name);
            var result = RegisterFailureResult(module, moduleType, exception, ModuleStatus.Failed, plan.Context);
            await CompleteCollectedResultAsync(result, moduleType, plan, result?.ExceptionOrDefault ?? exception)
                .ConfigureAwait(false);
            return null;
        }
        finally
        {
            if (!cleanupDeferredToResultTask)
            {
                _cacheResultRepository?.DiscardFingerprint(module);
            }
        }
    }

    private async Task<bool> TryRestoreCachedResultAsync(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        IExecutionBackendContext context,
        CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var cacheResultRepository = _cacheResultRepository;
        if (cacheResultRepository is null || !CanRestoreCachedResult(moduleState))
        {
            return false;
        }

        IModuleResult cachedResult;
        try
        {
            var scope = _serviceScopeFactory.CreateAsyncScope();
            await using var scopeLifetime = scope.ConfigureAwait(false);
            var pipelineContext = scope.ServiceProvider.GetRequiredService<IPipelineContext>();
            var candidate = await ModuleCacheResultAccessor.GetResultAsync(
                    cacheResultRepository,
                    module,
                    pipelineContext,
                    cancellationToken)
                .ConfigureAwait(false);
            if (candidate is null)
            {
                return false;
            }

            cachedResult = candidate;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogWarning(
                exception,
                "Could not restore module {Module} from cache on the coordinator; dispatching normally",
                moduleType.Name);
            return false;
        }

        await TryUploadCachedArtifactsAsync(moduleType, cancellationToken).ConfigureAwait(false);
        CompleteCachedResult(moduleState, scheduler, context, cachedResult);
        return true;
    }

    private void CompleteCachedResult(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        IExecutionBackendContext context,
        IModuleResult cachedResult)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var restoredResult = ModuleResultFactory.WithStatus(
            cachedResult,
            ModuleStatus.RestoredFromCache);
        var applied = context.TryApplyResult(module, restoredResult);
        var acceptedResult = (applied ? restoredResult : GetCompletedResult(module)) ?? throw new InvalidOperationException($"Module {moduleType.Name} rejected a cache result without a completed result.");
        moduleState.Result = acceptedResult;
        if (applied)
        {
            _cacheHitTracker.Record(acceptedResult);
            _logger.LogInformation(
                "Restored module {Module} from cache on the coordinator; distributed dispatch avoided",
                moduleType.Name);
        }

        scheduler.MarkModuleCompleted(
            moduleType,
            success: acceptedResult.ExceptionOrDefault is null,
            exception: acceptedResult.ExceptionOrDefault,
            statusOverride: acceptedResult.Status);
    }

    private async Task TryUploadCachedArtifactsAsync(
        Type moduleType,
        CancellationToken cancellationToken)
    {
        if (_artifactLifecycleManager is null)
        {
            return;
        }

        try
        {
            var artifacts = await _artifactLifecycleManager.UploadProducedArtifactsAsync(moduleType, cancellationToken)
                .ConfigureAwait(false);
            _acceptedArtifacts?.Record(ModuleId.FromType(moduleType), artifacts);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogWarning(
                exception,
                "Could not upload artifacts for cached module {Module}; using the cached result without republished artifacts",
                moduleType.Name);
        }
    }

    private bool CanRestoreCachedResult(ModuleState moduleState)
    {
        return _pipelineOptions?.Value.DisableModuleCache != true
               && moduleState.Module.Configuration.CacheEnabled
               && moduleState.Module.Configuration.SkipCondition is null
               && !moduleState.SkipResult.ShouldSkip
               && !moduleState.ModuleType.GetCustomAttributes(true).OfType<RunConditionAttribute>().Any();
    }

    internal static void CompleteCancelledModules(
        IModuleScheduler scheduler,
        IModuleResultRegistrar resultRegistrar,
        CancellationToken cancellationToken)
    {
        var cancelledModules = scheduler.CancelPendingModules();
        resultRegistrar.RegisterTerminatedResultsForCancelledModules(
            cancelledModules,
            new OperationCanceledException(cancellationToken));
    }

    internal static IModuleResult CreateCollectorFailureResult(
        IModule module,
        Type moduleType,
        Exception exception,
        ModuleStatus status)
    {
        var executionContext = new ModuleExecutionContext(module, moduleType)
        {
            Status = status,
            Exception = exception,
        };
        return ModuleResultFactory.CreateException(
            module.ResultType,
            exception,
            executionContext);
    }

    private async Task CompleteAlwaysRunModulesAsync(
        DistributedPlan plan,
        CancellationTokenSource masterWorkerCts)
    {
        try
        {
            if (plan.PipelineCts.IsCancellationRequested && !_lifetime.ApplicationStopping.IsCancellationRequested)
            {
                await _alwaysRunHandler.WaitForAlwaysRunModulesAsync(
                        plan.Scheduler,
                        plan.Modules,
                        moduleState => PublishAndCollectLateAlwaysRunModuleAsync(moduleState, plan))
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            if (!masterWorkerCts.IsCancellationRequested)
            {
                await masterWorkerCts.CancelAsync().ConfigureAwait(false);
            }
        }
    }

    private async Task PublishAndCollectLateAlwaysRunModuleAsync(ModuleState moduleState, DistributedPlan plan)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        ModuleAssignment assignment;
        try
        {
            assignment = await _publisher.CreateAssignmentAsync(
                    module,
                    _lifetime.ApplicationStopping,
                    moduleState.Priority,
                    moduleState.CriticalPathWeight,
                    plan.Modules)
                .ConfigureAwait(false);
        }
        catch (UnsatisfiableModuleRequirementException exception)
        {
            CompleteUnroutableModule(moduleState, plan.Scheduler, exception, plan.Context);
            return;
        }

        if (!plan.Scheduler.MarkModuleStarted(moduleType))
        {
            return;
        }

        await PublishAndCollectDistributedResultAsync(assignment, module, moduleType, plan).ConfigureAwait(false);
    }

    private async Task WaitForMinimumWorkersAsync(
        DateTimeOffset registrationDeadline,
        CancellationToken cancellationToken)
    {
        var expectedWorkers = Math.Max(0, _options.Value.TotalInstances - 1);
        var minimumWorkers = _options.Value.MinimumWorkerCount;
        if (minimumWorkers < 0 || minimumWorkers > expectedWorkers)
        {
            throw new InvalidOperationException(
                $"{nameof(DistributedOptions.MinimumWorkerCount)} must be between zero and " +
                $"{expectedWorkers} for the configured total instance count.");
        }

        if (minimumWorkers == 0)
        {
            return;
        }

        _logger.LogInformation(
            "Waiting for at least {Minimum} of {Expected} worker(s) to register (timeout: {Timeout})...",
            minimumWorkers,
            expectedWorkers,
            _options.Value.WorkerRegistrationTimeout);

        var lastCount = 0;
        while (DateTimeOffset.UtcNow < registrationDeadline)
        {
            var workers = await _masterCoordinator.GetRegisteredWorkersAsync(cancellationToken)
                .ConfigureAwait(false);
            ValidateWorkerSchemas(workers);
            if (workers.Count != lastCount)
            {
                lastCount = workers.Count;
                _logger.LogInformation("{Count}/{Expected} worker(s) registered", workers.Count, expectedWorkers);
            }

            if (workers.Count >= minimumWorkers)
            {
                _logger.LogInformation(
                    "Minimum worker count reached — starting work distribution with {Count} worker(s)",
                    workers.Count);
                return;
            }

            await DelayUntilNextWorkerCheckAsync(registrationDeadline, cancellationToken)
                .ConfigureAwait(false);
        }

        _logger.LogWarning(
            "Worker registration timeout ({Timeout} expired). {Count}/{Minimum} required worker(s) registered — proceeding with available workers",
            _options.Value.WorkerRegistrationTimeout,
            lastCount,
            minimumWorkers);
    }

    private async Task RunMasterWorkerLoopAsync(
        IReadOnlyList<IModule> modules,
        Dictionary<ModuleId, IModule> moduleLookup,
        IReadOnlySet<Capability> capabilities,
        int maxConcurrency,
        InFlightLeases inFlightLeases,
        CancellationToken pipelineCancellationToken,
        CancellationToken workerCancellationToken)
    {
        _logger.LogInformation("Coordinator worker loop started with capabilities: {Capabilities}",
            string.Join(", ", capabilities));

        var dependencyResultCache = new DependencyResultCache(_workerCoordinator, workerCancellationToken);
        _logger.LogDebug(
            "Coordinator worker loop starting {MaxConcurrency} concurrent execution slot(s)",
            maxConcurrency);

        await DistributedWorkerPool.RunAsync(
            DequeueForMasterAsync,
            (lease, claimedAt, _) =>
            {
                ArmResultDeadline(lease.Assignment.ModuleId);
                return ExecuteMasterAssignmentAsync(
                    lease,
                    claimedAt,
                    moduleLookup,
                    dependencyResultCache,
                    pipelineCancellationToken,
                    workerCancellationToken);
            },
            maxConcurrency,
            exception => _logger.LogError(exception, "Coordinator worker loop encountered an error"),
            workerCancellationToken,
            inFlightLeases: inFlightLeases).ConfigureAwait(false);

        // Stop waiting for new work as soon as the pipeline is cancelled, then keep dequeuing on
        // the worker lifetime: after a failure broadcast the coordinator only returns AlwaysRun
        // leases, which must still run while AlwaysRun teardown completes.
        async Task<ModuleLease?> DequeueForMasterAsync(CancellationToken token)
        {
            while (true)
            {
                var observePipelineCancellation = !pipelineCancellationToken.IsCancellationRequested;
                using var dequeueCts = observePipelineCancellation
                    ? CancellationTokenSource.CreateLinkedTokenSource(pipelineCancellationToken, token)
                    : null;
                try
                {
                    var lease = await _workerCoordinator.DequeueModuleAsync(
                            MasterWorkerId,
                            capabilities,
                            dequeueCts?.Token ?? token)
                        .ConfigureAwait(false);
                    if (lease is null && PipelineCancelledDuringDequeue(observePipelineCancellation, token))
                    {
                        continue;
                    }

                    return lease;
                }
                catch (OperationCanceledException) when (PipelineCancelledDuringDequeue(observePipelineCancellation, token))
                {
                    // Retry on the worker lifetime so late AlwaysRun assignments still run.
                }
            }
        }

        bool PipelineCancelledDuringDequeue(bool observedPipelineCancellation, CancellationToken token) =>
            observedPipelineCancellation
            && pipelineCancellationToken.IsCancellationRequested
            && !token.IsCancellationRequested;
    }

    private async Task ExecuteMasterAssignmentAsync(
        ModuleLease lease,
        DateTimeOffset claimedAt,
        Dictionary<ModuleId, IModule> moduleLookup,
        DependencyResultCache dependencyResultCache,
        CancellationToken pipelineCancellationToken,
        CancellationToken workerCancellationToken)
    {
        var assignment = lease.Assignment;
        var executionCancellationToken = assignment.AlwaysRun
            ? workerCancellationToken
            : pipelineCancellationToken;
        if (executionCancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Coordinator skipping cancelled module {Module}", assignment.ModuleId);
        }
        else
        {
            _logger.LogDebug("Coordinator executing module {Module} locally", assignment.ModuleId);
        }

        await _assignmentExecutor.ExecuteAsync(
                lease,
                claimedAt,
                moduleLookup,
                dependencyResultCache,
                executionCancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Renews the master's own leases, returns expired worker leases to the queue and starts result
    /// deadlines for modules workers have claimed.
    /// </summary>
    private async Task RunLeaseMaintenanceAsync(InFlightLeases inFlightLeases, CancellationToken cancellationToken)
    {
        var options = _options.Value;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(options.WorkerHeartbeatInterval, cancellationToken).ConfigureAwait(false);
                await _masterCoordinator.SendHeartbeatAsync(
                        new WorkerStatus
                        {
                            WorkerId = MasterWorkerId,
                            RunId = options.RunId,
                            InFlightModules = inFlightLeases.GetModuleIds(),
                        },
                        cancellationToken)
                    .ConfigureAwait(false);

                foreach (var moduleId in await _masterCoordinator.RequeueExpiredLeasesAsync(cancellationToken)
                             .ConfigureAwait(false))
                {
                    _logger.LogWarning(
                        "The lease on distributed module {Module} expired without a result; returned it to the queue",
                        moduleId);
                }

                foreach (var lease in await _masterCoordinator.GetActiveLeasesAsync(cancellationToken)
                             .ConfigureAwait(false))
                {
                    ArmResultDeadline(lease.Assignment.ModuleId);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Distributed lease maintenance failed; retrying");
            }
        }
    }

    private void ArmResultDeadline(ModuleId moduleId)
    {
        if (_resultDeadlines.TryGetValue(moduleId, out var deadline))
        {
            deadline.Arm();
        }
    }

    private async Task PublishAndCollectDistributedResultAsync(
        ModuleAssignment assignment,
        IModule module,
        Type moduleType,
        DistributedPlan plan)
    {
        var pipelineToken = module.Configuration.AlwaysRun
            ? _lifetime.ApplicationStopping
            : plan.PipelineCts.Token;
        using var deadline = new ResultDeadline(GetResultDeadline(module));
        _resultDeadlines[assignment.ModuleId] = deadline;
        using var lifecycleCts = CancellationTokenSource.CreateLinkedTokenSource(pipelineToken, deadline.Token);

        // A coordinator that never accepts the assignment gets the same backstop.
        using var publishCts = CancellationTokenSource.CreateLinkedTokenSource(pipelineToken);
        if (deadline.Backstop is { } publishBackstop)
        {
            publishCts.CancelAfter(publishBackstop);
        }

        var published = false;

        try
        {
            // Check the route before publishing, so an unroutable module never sits in the queue.
            await EnsureAssignmentHasExecutionRouteAsync(assignment, plan.MasterCapabilities, pipelineToken)
                .ConfigureAwait(false);
            _logger.LogDebug("Distributing module {Module} to workers", moduleType.Name);
            await _publisher.PublishAsync(assignment, publishCts.Token).ConfigureAwait(false);
            publishCts.CancelAfter(Timeout.InfiniteTimeSpan);
            published = true;
            await CollectResultAsync(module, moduleType, plan, lifecycleCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (
            (deadline.Token.IsCancellationRequested || publishCts.IsCancellationRequested)
            && !pipelineToken.IsCancellationRequested)
        {
            // The claimed module outlived every configured attempt plus the result timeout.
            await WithdrawAsync(assignment.ModuleId).ConfigureAwait(false);
            _logger.LogError(
                "Distributed module {Module} produced no result within {Timeout} of being claimed; the worker may have stalled",
                moduleType.Name,
                deadline.Backstop);
            var failureResult = RegisterFailureResult(
                module,
                moduleType,
                new TimeoutException(
                    $"Module {moduleType.Name} did not produce a result within {deadline.Backstop} of being claimed."),
                ModuleStatus.TimedOut,
                plan.Context);
            await CompleteCollectedResultAsync(failureResult, moduleType, plan).ConfigureAwait(false);
            await PublishFailureResultAsync(failureResult, moduleType).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            if (published)
            {
                // Withdraw queued work the pipeline no longer needs; a claimed copy is cancelled by
                // the failure broadcast and its late result is ignored.
                await WithdrawAsync(assignment.ModuleId).ConfigureAwait(false);
            }

            var result = RegisterFailureResult(module, moduleType, exception, ModuleStatus.Cancelled, plan.Context);
            plan.Scheduler.MarkModuleCompleted(
                moduleType,
                result is not null && result.ExceptionOrDefault is null,
                statusOverride: GetTerminalStatus(result));
        }
        catch (Exception ex)
        {
            if (published)
            {
                await WithdrawAsync(assignment.ModuleId).ConfigureAwait(false);
            }

            _logger.LogError(ex, "Failed to publish or collect distributed module {Module}", moduleType.Name);
            var failureResult = RegisterFailureResult(module, moduleType, ex, ModuleStatus.Failed, plan.Context);
            await CompleteCollectedResultAsync(failureResult, moduleType, plan, failureResult?.ExceptionOrDefault ?? ex)
                .ConfigureAwait(false);
            await PublishFailureResultAsync(failureResult, moduleType).ConfigureAwait(false);
        }
        finally
        {
            _resultDeadlines.TryRemove(new KeyValuePair<ModuleId, ResultDeadline>(assignment.ModuleId, deadline));
            _cacheResultRepository?.DiscardFingerprint(module);
        }
    }

    private async Task WithdrawAsync(ModuleId moduleId)
    {
        try
        {
            await _masterCoordinator.WithdrawAssignmentAsync(moduleId, CancellationToken.None).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Failed to withdraw queued distributed module {Module}", moduleId);
        }
    }

    /// <summary>
    /// Gets how long the master waits for a result after a worker claims the module: every
    /// configured attempt at the module's per-attempt timeout plus <see cref="DistributedOptions.ModuleResultTimeout"/>.
    /// The worker enforces the per-attempt timeout itself; this is only a backstop for a stalled worker.
    /// </summary>
    private TimeSpan? GetResultDeadline(IModule module)
    {
        var resultTimeout = _options.Value.ModuleResultTimeout;
        if (resultTimeout <= TimeSpan.Zero)
        {
            return null;
        }

        var pipelineOptions = _pipelineOptions?.Value;
        var configuration = module.Configuration;
        var perAttempt = configuration.Timeout ?? pipelineOptions?.DefaultModuleTimeout ?? TimeSpan.Zero;
        var attempts = 1 + (configuration.RetryConfiguration?.Count ?? pipelineOptions?.DefaultRetryCount ?? 0);
        return perAttempt > TimeSpan.Zero
            ? (perAttempt * attempts) + resultTimeout
            : resultTimeout;
    }

    private async Task EnsureAssignmentHasExecutionRouteAsync(
        ModuleAssignment assignment,
        IReadOnlySet<Capability> masterCapabilities,
        CancellationToken cancellationToken)
    {
        if (assignment.RequiredCapabilities.IsSatisfiedBy(masterCapabilities))
        {
            return;
        }

        var registrationDeadline = DateTimeOffset.UtcNow + _options.Value.WorkerRegistrationTimeout;
        var expectedWorkers = Math.Max(0, _options.Value.TotalInstances - 1);
        IReadOnlyList<WorkerRegistration> workers;
        do
        {
            workers = await _masterCoordinator.GetRegisteredWorkersAsync(cancellationToken)
                .ConfigureAwait(false);
            ValidateWorkerSchemas(workers);
            if (workers.Any(worker => assignment.RequiredCapabilities.IsSatisfiedBy(worker.Capabilities)))
            {
                return;
            }

            if (workers.Count >= expectedWorkers || DateTimeOffset.UtcNow >= registrationDeadline)
            {
                break;
            }

            await DelayUntilNextWorkerCheckAsync(registrationDeadline, cancellationToken)
                .ConfigureAwait(false);
        }
        while (true);

        throw new DistributedRoutingException(
            assignment.ModuleId,
            assignment.RequiredCapabilities,
            workers.Count);
    }

    private void ValidateWorkerSchemas(IReadOnlyList<WorkerRegistration> workers)
    {
        var schema = _typeRegistry.GetPipelineSchemaVersion();
        foreach (var worker in workers)
        {
            PipelineSchemaVersionValidator.Validate(schema, worker.PipelineSchemaVersion, $"worker {worker.WorkerId}");
        }
    }

    private static async Task DelayUntilNextWorkerCheckAsync(
        DateTimeOffset registrationDeadline,
        CancellationToken cancellationToken)
    {
        var remaining = registrationDeadline - DateTimeOffset.UtcNow;
        if (remaining <= TimeSpan.Zero)
        {
            return;
        }

        await Task.Delay(
                remaining < WorkerRegistrationPollInterval
                    ? remaining
                    : WorkerRegistrationPollInterval,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task CollectResultAsync(
        IModule module,
        Type moduleType,
        DistributedPlan plan,
        CancellationToken cancellationToken)
    {
        var result = await _resultCollector.WaitForResultAsync(ModuleId.FromType(moduleType), cancellationToken)
            .ConfigureAwait(false);
        if (result is not null)
        {
            result = ApplyResult(module, result, plan.Context);
        }

        await CompleteCollectedResultAsync(result, moduleType, plan).ConfigureAwait(false);
        RecordReportedExecutionWindow(moduleType, result);
    }

    private async Task CompleteCollectedResultAsync(
        IModuleResult? result,
        Type moduleType,
        DistributedPlan plan,
        Exception? schedulerException = null)
    {
        var success = result is not null && result.ExceptionOrDefault is null;
        plan.Scheduler.MarkModuleCompleted(
            moduleType,
            success,
            success ? null : schedulerException,
            GetTerminalStatus(result));
        if (!success)
        {
            _logger.LogError("Distributed module {Module} failed — cancelling pipeline", moduleType.Name);
            plan.RequestFailureCancellation();
            await plan.PipelineCts.CancelAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Returns the collected result's status so the summary reports skipped, ignored, and other
    /// outcomes as they happened; non-terminal statuses fall back to the scheduler's inference.
    /// </summary>
    private static ModuleStatus? GetTerminalStatus(IModuleResult? result) =>
        result?.Status is null
            or ModuleStatus.NotStarted
            or ModuleStatus.Running
            or ModuleStatus.Unknown
            ? null
            : result.Status;

    /// <summary>
    /// Shortens the coordinator's dispatch-to-collection window to the execution time the executing
    /// process reported, so the summary shows module run time rather than queue wait. Only the
    /// reported duration is used; worker timestamps come from another clock.
    /// </summary>
    private void RecordReportedExecutionWindow(Type moduleType, IModuleResult? result)
    {
        if (_metricsCollector is null
            || result is null
            || result.StartTime == default
            || result.EndTime < result.StartTime)
        {
            return;
        }

        _metricsCollector.RecordReportedExecutionDuration(moduleType, result.EndTime - result.StartTime);
    }

    private static IModuleResult? GetCompletedResult(IModule module)
    {
        var task = module.AsInternal().ResultTask;
        return task.IsCompletedSuccessfully ? task.Result : null;
    }

    private static IModuleResult? ApplyResult(
        IModule module,
        IModuleResult result,
        IExecutionBackendContext context) =>
        context.TryApplyResult(module, result) ? result : GetCompletedResult(module);

    /// <summary>
    /// Completes a module no worker can run as skipped, both in the result registry and on the scheduler.
    /// </summary>
    private void CompleteUnroutableModule(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        UnsatisfiableModuleRequirementException exception,
        IExecutionBackendContext context)
    {
        var moduleType = moduleState.ModuleType;
        _logger.LogInformation("Skipping distributed module {Module}: {Reason}", moduleType.Name, exception.Message);
        var skipped = RegisterSkippedResult(moduleState.Module, moduleType, exception.SkipDecision, context);
        moduleState.Result = skipped;
        scheduler.MarkModuleCompleted(
            moduleType,
            success: true,
            statusOverride: skipped?.Status ?? ModuleStatus.Skipped);
    }

    private IModuleResult? RegisterSkippedResult(
        IModule module,
        Type moduleType,
        SkipDecision skipDecision,
        IExecutionBackendContext context)
    {
        var executionContext = new ModuleExecutionContext(module, moduleType)
        {
            Status = ModuleStatus.Skipped,
            SkipResult = skipDecision,
        };
        return ApplyResult(module, ModuleResultFactory.CreateSkipped(module.ResultType, executionContext), context);
    }

    private IModuleResult? RegisterFailureResult(
        IModule module,
        Type moduleType,
        Exception exception,
        ModuleStatus status,
        IExecutionBackendContext context)
    {
        try
        {
            var failureResult = CreateCollectorFailureResult(
                module,
                moduleType,
                exception,
                status);
            return ApplyResult(module, failureResult, context);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to register failure result for module {Module}", moduleType.Name);
            return null;
        }
    }

    private async Task PublishFailureResultAsync(IModuleResult? failureResult, Type moduleType)
    {
        if (failureResult is null)
        {
            return;
        }

        try
        {
            var serialized = _serializer.Serialize(
                failureResult,
                ModuleId.FromType(moduleType),
                MasterWorkerId);
            await _masterCoordinator.PublishResultAsync(serialized, lease: null, CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(
                ex,
                "Failed to publish failure result for module {Module}; pipeline cancellation has already been requested",
                moduleType.Name);
        }
    }

    /// <summary>
    /// The per-run state shared by the dispatch paths.
    /// </summary>
    private sealed record DistributedPlan(
        IModuleScheduler Scheduler,
        CancellationTokenSource PipelineCts,
        IExecutionBackendContext Context,
        Action RequestFailureCancellation,
        IReadOnlySet<Capability> MasterCapabilities,
        IReadOnlyList<IModule> Modules);

    /// <summary>
    /// Broadcasts <see cref="DistributedCancellationReason.PipelineFailed"/> the first time a failure
    /// is requested, so workers stop non-AlwaysRun work immediately rather than at shutdown.
    /// </summary>
    private sealed class FailureBroadcast(DistributedModuleExecutor executor)
    {
        private readonly TaskCompletionSource _broadcast = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _requested;

        public void Request()
        {
            if (Interlocked.Exchange(ref _requested, 1) != 0)
            {
                return;
            }

            _ = BroadcastAsync();
        }

        public Task WaitAsync() => Volatile.Read(ref _requested) == 0 ? Task.CompletedTask : _broadcast.Task;

        private async Task BroadcastAsync()
        {
            try
            {
                await executor.BroadcastCancellationAsync(DistributedCancellationReason.PipelineFailed)
                    .ConfigureAwait(false);
            }
            finally
            {
                _broadcast.TrySetResult();
            }
        }
    }

    /// <summary>
    /// The master's backstop deadline for one module's result. It starts when a worker is first
    /// seen holding the module's lease, not when the module is queued.
    /// </summary>
    private sealed class ResultDeadline(TimeSpan? backstop) : IDisposable
    {
        private readonly CancellationTokenSource _cts = new();
        private int _armed;

        public TimeSpan? Backstop { get; } = backstop;

        public CancellationToken Token => _cts.Token;

        public void Arm()
        {
            if (Backstop is not { } backstop || Interlocked.Exchange(ref _armed, 1) != 0)
            {
                return;
            }

            try
            {
                _cts.CancelAfter(backstop);
            }
            catch (ObjectDisposedException)
            {
                // The result arrived while the lease was being observed.
            }
        }

        public void Dispose() => _cts.Dispose();
    }
}

internal sealed class DistributedRoutingException(
    ModuleId moduleId,
    CapabilityRequirement requiredCapabilities,
    int registeredWorkerCount)
    : InvalidOperationException(
        $"No execution route is available for distributed module {moduleId}. " +
        $"Required capabilities: {requiredCapabilities}. " +
        $"Registered external workers: {registeredWorkerCount}.");
