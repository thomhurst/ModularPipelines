using System.Runtime.ExceptionServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
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
    DistributedCacheHitTracker? cacheHitTracker = null) : IExecutionBackend
{
    private static readonly TimeSpan WorkerRegistrationPollInterval = TimeSpan.FromMilliseconds(250);

    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IModuleSchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly IModuleRunner _moduleRunner = moduleRunner;
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

    public bool OwnsEntirePlan => true;

    public async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IExecutionBackendContext context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (modules.Count == 0)
        {
            return Array.Empty<IModuleResult>();
        }

        var workerMaxConcurrency = DistributedWorkerPool.GetMaxConcurrency(
            _parallelLimitProvider,
            _options.Value);

        // Register all module types in the type registry for serialization
        foreach (var module in modules)
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
        var failureCancellationRequested = 0;
        Action requestFailureCancellation = () =>
            Interlocked.Exchange(ref failureCancellationRequested, 1);
        try
        {
            // Wait for workers to register before distributing work. Keep this inside the
            // shutdown scope so cancellation or coordinator failure still notifies workers.
            var options = _options.Value;
            var registrationDeadline = DateTimeOffset.UtcNow + options.CapabilityTimeout;
            await WaitForMinimumWorkersAsync(registrationDeadline, executionCts.Token)
                .ConfigureAwait(false);
            var masterCapabilities = BuildCapabilities(options);

            scheduler = _schedulerFactory.Create();
            scheduler.InitializeModules(modules);
            UsedHistoryModuleSchedulerInitializer.Precomplete(
                modules,
                scheduler,
                _resultRegistry);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(executionCts.Token);
            using var masterWorkerCts = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.ApplicationStopping);
            var executionToken = cts.Token;
            using var cancellationRegistration = executionToken.Register(
                () => CompleteCancelledModules(scheduler, _resultRegistrar, executionToken));

            var schedulerTask = scheduler.RunSchedulerAsync(executionToken);

            // Start the master worker loop — the master participates as a worker,
            // dequeuing and executing modules from the same queue as external workers.
            var masterWorkerTask = RunMasterWorkerLoopAsync(
                modules,
                moduleLookup,
                workerMaxConcurrency,
                masterCapabilities,
                executionToken,
                masterWorkerCts.Token);

            var resultTasks = await PublishReadyModulesAsync(
                    scheduler,
                    cts,
                    context,
                    requestFailureCancellation,
                    masterCapabilities)
                .ConfigureAwait(false);
            await IgnoreCancellationAsync(Task.WhenAll(resultTasks)).ConfigureAwait(false);
            await FinalizeExecutionAsync(
                    scheduler,
                    modules,
                    cts,
                    masterWorkerCts,
                    masterWorkerTask,
                    schedulerTask,
                    context,
                    requestFailureCancellation,
                    masterCapabilities)
                .ConfigureAwait(false);
        }
        catch
        {
            requestFailureCancellation();
            throw;
        }
        finally
        {
            await SignalWorkerShutdownAsync(
                    _lifetime.ApplicationStopping.IsCancellationRequested
                    || Volatile.Read(ref failureCancellationRequested) != 0)
                .ConfigureAwait(false);
            scheduler?.Dispose();
        }

        return modules
            .Select(module => _resultRegistry.GetResult(module.GetType()))
            .OfType<IModuleResult>()
            .ToArray();
    }

    internal Task<IReadOnlyList<IModuleResult>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        return ExecuteAsync(
            modules,
            new ExecutionBackendContext(_resultRegistry),
            CancellationToken.None);
    }

    private async Task<IReadOnlyList<Task>> PublishReadyModulesAsync(
        IModuleScheduler scheduler,
        CancellationTokenSource pipelineCts,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        IReadOnlySet<Capability> masterCapabilities)
    {
        var resultTasks = new List<Task>();
        try
        {
            await foreach (var moduleState in scheduler.ReadyModules.ReadAllAsync(pipelineCts.Token))
            {
                resultTasks.Add(RestoreOrExecuteDistributedModuleAsync(
                    moduleState,
                    scheduler,
                    pipelineCts,
                    context,
                    requestFailureCancellation,
                    masterCapabilities));
            }
        }
        catch (OperationCanceledException)
        {
            // Expected during shutdown.
        }

        return resultTasks;
    }

    private async Task RestoreOrExecuteDistributedModuleAsync(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        CancellationTokenSource pipelineCts,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        IReadOnlySet<Capability> masterCapabilities)
    {
        pipelineCts.Token.ThrowIfCancellationRequested();

        if (await TryRestoreCachedResultAsync(moduleState, scheduler, pipelineCts.Token)
                .ConfigureAwait(false))
        {
            return;
        }

        var module = moduleState.Module;
        var collectTask = await TryStartDistributedExecutionAsync(
                module,
                module.GetType(),
                scheduler,
                pipelineCts,
                context,
                requestFailureCancellation,
                masterCapabilities)
            .ConfigureAwait(false);
        if (collectTask is not null)
        {
            await collectTask.ConfigureAwait(false);
        }
    }

    private async Task FinalizeExecutionAsync(
        IModuleScheduler scheduler,
        IReadOnlyList<IModule> modules,
        CancellationTokenSource pipelineCts,
        CancellationTokenSource masterWorkerCts,
        Task masterWorkerTask,
        Task schedulerTask,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        IReadOnlySet<Capability> masterCapabilities)
    {
        Exception? alwaysRunException = null;
        try
        {
            await CompleteAlwaysRunModulesAsync(
                    scheduler,
                    modules,
                    pipelineCts,
                    masterWorkerCts,
                    context,
                    requestFailureCancellation,
                    masterCapabilities)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            requestFailureCancellation();
            alwaysRunException = exception;
        }

        if (!pipelineCts.IsCancellationRequested)
        {
            await pipelineCts.CancelAsync();
        }

        await IgnoreCancellationAsync(masterWorkerTask).ConfigureAwait(false);
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

    private async Task SignalWorkerShutdownAsync(bool broadcastCancellation)
    {
        // Always signal workers to stop — whether the master succeeded or crashed.
        // Without this, workers hang forever waiting for work that will never come.
        if (broadcastCancellation)
        {
            try
            {
                await _masterCoordinator.BroadcastCancellationAsync(CancellationToken.None)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to broadcast cancellation to workers during shutdown");
            }
        }

        try
        {
            await _masterCoordinator.SignalCompletionAsync(CancellationToken.None).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to signal completion to workers during shutdown");
        }
    }

    private async Task<Task?> TryStartDistributedExecutionAsync(
        IModule module,
        Type moduleType,
        IModuleScheduler scheduler,
        CancellationTokenSource cts,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        IReadOnlySet<Capability> masterCapabilities)
    {
        var cleanupDeferredToResultTask = false;
        try
        {
            var assignment = await _publisher.CreateAssignmentAsync(module, cts.Token)
                .ConfigureAwait(false);
            if (!scheduler.MarkModuleStarted(moduleType))
            {
                return null;
            }

            cleanupDeferredToResultTask = true;
            return PublishAndCollectDistributedResultAsync(
                assignment,
                module,
                moduleType,
                scheduler,
                cts,
                context,
                requestFailureCancellation,
                masterCapabilities);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            return null;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to create a distributed assignment for module {Module}",
                moduleType.Name);
            RegisterFailureResult(module, moduleType, exception, ModuleStatus.Failed, context);
            scheduler.MarkModuleCompleted(moduleType, false, exception);
            requestFailureCancellation();
            await cts.CancelAsync().ConfigureAwait(false);
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
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
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
                "Could not restore module {Module} from cache on the master; dispatching normally",
                moduleType.Name);
            return false;
        }

        await TryUploadCachedArtifactsAsync(moduleType, cancellationToken).ConfigureAwait(false);

        var restoredResult = ModuleResultFactory.WithStatus(
            cachedResult,
            ModuleStatus.RestoredFromCache);
        moduleState.Result = restoredResult;
        _resultRegistry.RegisterResult(moduleType, restoredResult);
        ModuleCompletionSourceApplicator.TryApply(module, restoredResult);
        _cacheHitTracker.Record(restoredResult);
        scheduler.MarkModuleCompleted(
            moduleType,
            success: true,
            statusOverride: ModuleStatus.RestoredFromCache);
        _logger.LogInformation(
            "Restored module {Module} from cache on the master; distributed dispatch avoided",
            moduleType.Name);
        return true;
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
            await _artifactLifecycleManager.UploadProducedArtifactsAsync(moduleType, cancellationToken)
                .ConfigureAwait(false);
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
               && !moduleState.ModuleType.GetCustomAttributes(true).OfType<IConditionAttribute>().Any();
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
        IModuleScheduler scheduler,
        IReadOnlyList<IModule> modules,
        CancellationTokenSource pipelineCts,
        CancellationTokenSource masterWorkerCts,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        IReadOnlySet<Capability> masterCapabilities)
    {
        try
        {
            if (pipelineCts.IsCancellationRequested && !_lifetime.ApplicationStopping.IsCancellationRequested)
            {
                await _alwaysRunHandler.WaitForAlwaysRunModulesAsync(
                        scheduler,
                        modules,
                        moduleState => PublishAndCollectLateAlwaysRunModuleAsync(
                            moduleState,
                            scheduler,
                            pipelineCts,
                            context,
                            requestFailureCancellation,
                            masterCapabilities))
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

    private async Task PublishAndCollectLateAlwaysRunModuleAsync(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        CancellationTokenSource pipelineCts,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        IReadOnlySet<Capability> masterCapabilities)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var assignment = await _publisher.CreateAssignmentAsync(
                module,
                _lifetime.ApplicationStopping)
            .ConfigureAwait(false);
        if (!scheduler.MarkModuleStarted(moduleType))
        {
            return;
        }

        await PublishAndCollectDistributedResultAsync(
                assignment,
                module,
                moduleType,
                scheduler,
                pipelineCts,
                context,
                requestFailureCancellation,
                masterCapabilities)
            .ConfigureAwait(false);
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
            _options.Value.CapabilityTimeout);

        var lastCount = 0;
        while (DateTimeOffset.UtcNow < registrationDeadline)
        {
            var workers = await _masterCoordinator.GetRegisteredWorkersAsync(cancellationToken)
                .ConfigureAwait(false);
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
            _options.Value.CapabilityTimeout,
            lastCount,
            minimumWorkers);
    }

    private async Task RunMasterWorkerLoopAsync(
        IReadOnlyList<IModule> modules,
        Dictionary<string, IModule> moduleLookup,
        int maxConcurrency,
        IReadOnlySet<Capability> capabilities,
        CancellationToken pipelineCancellationToken,
        CancellationToken workerCancellationToken)
    {
        _logger.LogInformation("Master worker loop started with capabilities: {Capabilities}",
            string.Join(", ", capabilities));

        _logger.LogInformation(
            "Master worker loop starting {MaxConcurrency} concurrent execution slot(s)",
            maxConcurrency);
        await DistributedWorkerPool.RunAsync(
            token => _workerCoordinator.DequeueModuleAsync(capabilities, token),
            async (assignment, _) =>
            {
                if (pipelineCancellationToken.IsCancellationRequested && !assignment.Configuration.AlwaysRun)
                {
                    _logger.LogInformation(
                        "Master skipping cancelled module {Module}",
                        assignment.ModuleTypeName);
                    return;
                }

                _logger.LogInformation("Master executing module {Module} locally",
                    assignment.ModuleTypeName);

                var executionCancellationToken = assignment.Configuration.AlwaysRun
                    ? workerCancellationToken
                    : pipelineCancellationToken;
                await ExecuteAssignmentAsync(
                    assignment,
                    modules,
                    moduleLookup,
                    executionCancellationToken).ConfigureAwait(false);
            },
            maxConcurrency,
            exception => _logger.LogError(exception, "Master worker loop encountered an error"),
            workerCancellationToken).ConfigureAwait(false);
    }

    private async Task ExecuteAssignmentAsync(
        ModuleAssignment assignment,
        IReadOnlyList<IModule> modules,
        Dictionary<string, IModule> moduleLookup,
        CancellationToken cancellationToken)
    {
        var resolved = _typeRegistry.Resolve(assignment.ModuleTypeName);
        if (resolved is null)
        {
            _logger.LogError("Cannot resolve module type: {Type}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
            await DependencyResultApplicator.PublishResolutionFailureAsync(assignment, _options.Value.InstanceIndex, _workerCoordinator, _logger, cancellationToken);
            return;
        }

        if (!moduleLookup.TryGetValue(assignment.ModuleTypeName, out var module))
        {
            _logger.LogError("Module instance not found: {Type}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
            await DependencyResultApplicator.PublishResolutionFailureAsync(assignment, _options.Value.InstanceIndex, _workerCoordinator, _logger, cancellationToken);
            return;
        }

        // Apply dependency results so that GetModule<T>() works
        if (assignment.DependencyResults is { Count: > 0 })
        {
            DependencyResultApplicator.Apply(
                assignment.DependencyResults,
                moduleLookup,
                _serializer,
                _resultRegistry,
                _logger);
        }

        try
        {
            await ExecuteAndPublishAsync(assignment, module, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Module {Module} execution failed on master", assignment.ModuleTypeName);
            await PublishFailureAsync(assignment, resolved.Value.ResultType, module, ex, cancellationToken);
        }
    }

    private async Task ExecuteAndPublishAsync(
        ModuleAssignment assignment,
        IModule module,
        CancellationToken cancellationToken)
    {
        var moduleType = module.GetType();
        await using var serviceScope = _serviceScopeFactory.CreateAsyncScope();
        var moduleLogger = serviceScope.ServiceProvider
            .GetRequiredService<IInternalModuleLoggerAccessor>()
            .GetLogger(moduleType) as IInternalModuleLogger
            ?? throw new InvalidOperationException($"No internal module logger is available for {moduleType.Name}.");
        using var outputScope = new ModuleOutputContextScope(moduleType, moduleLogger);

        try
        {
            if (_artifactLifecycleManager is not null)
            {
                await _artifactLifecycleManager.DownloadConsumedArtifactsAsync(moduleType, cancellationToken);
            }

            var moduleState = new ModuleState(module, moduleType);
            ModuleStateDependencyInitializer.Populate(
                moduleState,
                _typeRegistry.GetRegisteredModuleTypes(),
                _dependencyRegistry,
                _metadataRegistry);
            using (DistributedAssignmentExecutionScope.Enter())
            {
                await _moduleRunner.ExecuteWithoutDependencyWaitAsync(
                    moduleState,
                    cancellationToken).ConfigureAwait(false);
            }

            var result = await module.AsInternal().ResultTask;
            var artifactReferences = await TryUploadArtifactsAsync(
                module,
                assignment.ModuleTypeName,
                moduleLogger,
                cancellationToken);
            if (result is null)
            {
                return;
            }

            var serialized = _serializer.Serialize(
                result,
                assignment.ModuleTypeName,
                assignment.ResultTypeName,
                _options.Value.InstanceIndex);
            if (artifactReferences is not null)
            {
                serialized = serialized with { Artifacts = artifactReferences };
            }

            await _workerCoordinator.PublishResultAsync(serialized, cancellationToken);
        }
        catch (Exception ex)
        {
            moduleLogger.SetException(ex);
            throw;
        }
    }

    private async Task<IReadOnlyList<ArtifactReference>?> TryUploadArtifactsAsync(
        IModule module,
        string moduleTypeName,
        IModuleLogger moduleLogger,
        CancellationToken cancellationToken)
    {
        if (_artifactLifecycleManager is null)
        {
            return null;
        }

        try
        {
            var artifactReferences = await _artifactLifecycleManager.UploadProducedArtifactsAsync(module.GetType(), cancellationToken);
            return artifactReferences.Count == 0 ? null : artifactReferences;
        }
        catch (Exception ex)
        {
            moduleLogger.LogError(ex, "Failed to upload artifacts for {Module}", moduleTypeName);
            return null;
        }
    }

    private async Task PublishFailureAsync(
        ModuleAssignment assignment,
        Type resultType,
        IModule module,
        Exception exception,
        CancellationToken cancellationToken)
    {
        try
        {
            var failureResult = ModuleResultFactory.CreateException(
                resultType,
                exception,
                new ModuleExecutionContext(module, module.GetType()));
            var serialized = _serializer.Serialize(
                failureResult,
                assignment.ModuleTypeName,
                assignment.ResultTypeName,
                _options.Value.InstanceIndex);
            await _workerCoordinator.PublishResultAsync(serialized, cancellationToken);
        }
        catch (Exception publishException)
        {
            _logger.LogCritical(publishException, "Failed to publish failure result for {Module}", assignment.ModuleTypeName);
        }
    }

    private async Task PublishAndCollectDistributedResultAsync(
        ModuleAssignment assignment,
        IModule module,
        Type moduleType,
        IModuleScheduler scheduler,
        CancellationTokenSource cts,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        IReadOnlySet<Capability> masterCapabilities)
    {
        var pipelineToken = module.Configuration.AlwaysRun
            ? _lifetime.ApplicationStopping
            : cts.Token;
        using var timeoutCts = CreateResultTimeoutSource(module.Configuration.Timeout, pipelineToken);
        var lifecycleToken = timeoutCts?.Token ?? pipelineToken;

        try
        {
            _logger.LogInformation("Distributing module {Module} to workers", moduleType.Name);
            await _publisher.PublishAsync(assignment, lifecycleToken).ConfigureAwait(false);
            await EnsureAssignmentHasExecutionRouteAsync(
                    assignment,
                    masterCapabilities,
                    pipelineToken)
                .ConfigureAwait(false);
            await CollectResultAsync(
                module,
                moduleType,
                scheduler,
                cts,
                context,
                requestFailureCancellation,
                lifecycleToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (
            timeoutCts?.IsCancellationRequested == true
            && !pipelineToken.IsCancellationRequested)
        {
            // Timeout expired (not pipeline cancellation)
            _logger.LogError("Distributed module {Module} timed out waiting for result — worker may have died", moduleType.Name);
            RegisterFailureResult(
                module,
                moduleType,
                new TimeoutException(
                    $"Module {moduleType.Name} did not produce a result within the configured timeout"),
                ModuleStatus.TimedOut,
                context);
            scheduler.MarkModuleCompleted(moduleType, false);
            requestFailureCancellation();
            await cts.CancelAsync().ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
        {
            _resultRegistrar.RegisterTerminatedResult(module, moduleType, exception);
            scheduler.MarkModuleCompleted(moduleType, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish or collect distributed module {Module}", moduleType.Name);
            RegisterFailureResult(module, moduleType, ex, ModuleStatus.Failed, context);
            scheduler.MarkModuleCompleted(moduleType, false, ex);
            requestFailureCancellation();
            await cts.CancelAsync().ConfigureAwait(false);
        }
        finally
        {
            _cacheResultRepository?.DiscardFingerprint(module);
        }
    }

    private async Task EnsureAssignmentHasExecutionRouteAsync(
        ModuleAssignment assignment,
        IReadOnlySet<Capability> masterCapabilities,
        CancellationToken cancellationToken)
    {
        if (CapabilityMatcher.CanExecute(assignment, masterCapabilities))
        {
            return;
        }

        var registrationDeadline = DateTimeOffset.UtcNow + _options.Value.CapabilityTimeout;
        var expectedWorkers = Math.Max(0, _options.Value.TotalInstances - 1);
        IReadOnlyList<WorkerRegistration> workers;
        do
        {
            workers = await _masterCoordinator.GetRegisteredWorkersAsync(cancellationToken)
                .ConfigureAwait(false);
            if (workers.Any(worker => CapabilityMatcher.CanExecute(assignment, worker)))
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
            assignment.ModuleTypeName,
            assignment.RequiredCapabilities,
            workers.Count);
    }

    private static HashSet<Capability> BuildCapabilities(DistributedOptions options)
    {
        var capabilities = new HashSet<Capability>(options.Capabilities);
        if (options.AutoDetectOsCapability)
        {
            capabilities.UnionWith(OsCapabilityDetector.Detect());
        }

        return capabilities;
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

    private CancellationTokenSource? CreateResultTimeoutSource(TimeSpan? moduleTimeout, CancellationToken cancellationToken)
    {
        var timeout = moduleTimeout;
        if (timeout is null && _options.Value.ModuleResultTimeout > TimeSpan.Zero)
        {
            timeout = _options.Value.ModuleResultTimeout;
        }

        if (timeout is null)
        {
            return null;
        }

        var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(timeout.Value);
        return timeoutCts;
    }

    private async Task CollectResultAsync(
        IModule module,
        Type moduleType,
        IModuleScheduler scheduler,
        CancellationTokenSource pipelineCts,
        IExecutionBackendContext context,
        Action requestFailureCancellation,
        CancellationToken cancellationToken)
    {
        var result = await _resultCollector.WaitForResultAsync(moduleType.FullName!, cancellationToken);
        var success = result is not null && result.ExceptionOrDefault is null;

        if (result is not null)
        {
            context.TryApplyResult(module, result);
        }

        scheduler.MarkModuleCompleted(moduleType, success);
        if (!success)
        {
            _logger.LogError("Distributed module {Module} failed on worker — cancelling pipeline", moduleType.Name);
            requestFailureCancellation();
            await pipelineCts.CancelAsync();
        }
    }

    private void RegisterFailureResult(
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
            context.TryApplyResult(module, failureResult);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to register failure result for module {Module}", moduleType.Name);
        }
    }
}

internal sealed class DistributedRoutingException(
    string moduleTypeName,
    IReadOnlySet<Capability> requiredCapabilities,
    int registeredWorkerCount)
    : InvalidOperationException(
        $"No execution route is available for distributed module {moduleTypeName}. " +
        $"Required capabilities: [{string.Join(", ", requiredCapabilities)}]. " +
        $"Registered external workers: {registeredWorkerCount}.");
