using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

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
    IServiceScopeFactory serviceScopeFactory,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<DistributedModuleExecutor> logger) : IModuleExecutor
{
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
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ArtifactLifecycleManager? _artifactLifecycleManager = artifactLifecycleManager;
    private readonly ILogger<DistributedModuleExecutor> _logger = logger;

    public async Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        if (modules.Count == 0)
        {
            return modules;
        }

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

        IModuleScheduler? scheduler = null;
        var failureCancellationRequested = 0;
        Action requestFailureCancellation = () =>
            Interlocked.Exchange(ref failureCancellationRequested, 1);
        try
        {
            // Wait for workers to register before distributing work. Keep this inside the
            // shutdown scope so cancellation or coordinator failure still notifies workers.
            await WaitForWorkersAsync(_lifetime.ApplicationStopping).ConfigureAwait(false);

            scheduler = _schedulerFactory.Create();
            scheduler.InitializeModules(modules);
            UsedHistoryModuleSchedulerInitializer.Precomplete(
                modules,
                scheduler,
                _resultRegistry);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.ApplicationStopping);
            using var masterWorkerCts = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.ApplicationStopping);
            cts.Token.Register(() => CompleteCancelledModules(scheduler, _resultRegistrar, cts.Token));

            var schedulerTask = scheduler.RunSchedulerAsync(cts.Token);
            var resultTasks = new List<Task>();

            // Start the master worker loop — the master participates as a worker,
            // dequeuing and executing modules from the same queue as external workers.
            var masterWorkerTask = RunMasterWorkerLoopAsync(
                modules,
                moduleLookup,
                cts.Token,
                masterWorkerCts.Token);

            try
            {
                await foreach (var moduleState in scheduler.ReadyModules.ReadAllAsync(cts.Token))
                {
                    var moduleType = moduleState.Module.GetType();
                    var assignment = await _publisher.CreateAssignmentAsync(
                            moduleState.Module,
                            cts.Token)
                        .ConfigureAwait(false);
                    if (!scheduler.MarkModuleStarted(moduleType))
                    {
                        continue;
                    }

                    var collectTask = PublishAndCollectDistributedResultAsync(
                        assignment,
                        moduleState.Module,
                        moduleType,
                        scheduler,
                        cts,
                        requestFailureCancellation);
                    resultTasks.Add(collectTask);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected during shutdown
            }

            try
            {
                await Task.WhenAll(resultTasks).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected when a module failure cancels the pipeline
            }

            await CompleteAlwaysRunModulesAsync(scheduler, modules, cts, masterWorkerCts)
                .ConfigureAwait(false);

            // All results collected — stop the scheduler after successful execution.
            if (!cts.IsCancellationRequested)
            {
                await cts.CancelAsync();
            }

            try
            {
                await masterWorkerTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected — master worker loop exits on cancellation
            }

            try
            {
                await schedulerTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected
            }
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

        return modules;
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
        CancellationTokenSource masterWorkerCts)
    {
        try
        {
            if (pipelineCts.IsCancellationRequested && !_lifetime.ApplicationStopping.IsCancellationRequested)
            {
                await _alwaysRunHandler.WaitForAlwaysRunModulesAsync(scheduler, modules).ConfigureAwait(false);
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

    private async Task WaitForWorkersAsync(CancellationToken cancellationToken)
    {
        var expectedWorkers = _options.Value.TotalInstances - 1;
        if (expectedWorkers <= 0)
        {
            return;
        }

        var timeout = _options.Value.CapabilityTimeout;
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(timeout);

        _logger.LogInformation("Waiting for {Expected} worker(s) to register (timeout: {Timeout})...",
            expectedWorkers, timeout);

        var lastCount = 0;
        while (!timeoutCts.IsCancellationRequested)
        {
            try
            {
                var workers = await _masterCoordinator.GetRegisteredWorkersAsync(timeoutCts.Token);
                if (workers.Count != lastCount)
                {
                    lastCount = workers.Count;
                    _logger.LogInformation("{Count}/{Expected} worker(s) registered", workers.Count, expectedWorkers);
                }

                if (workers.Count >= expectedWorkers)
                {
                    _logger.LogInformation("All {Expected} worker(s) registered — starting work distribution", expectedWorkers);
                    return;
                }

                await Task.Delay(TimeSpan.FromSeconds(2), timeoutCts.Token);
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                // Timeout expired, but pipeline not cancelled — proceed with available workers
                _logger.LogWarning(
                    "Worker registration timeout ({Timeout} expired). {Count}/{Expected} worker(s) registered — proceeding with available workers",
                    timeout, lastCount, expectedWorkers);
                return;
            }
        }
    }

    private async Task RunMasterWorkerLoopAsync(
        IReadOnlyList<IModule> modules,
        Dictionary<string, IModule> moduleLookup,
        CancellationToken pipelineCancellationToken,
        CancellationToken workerCancellationToken)
    {
        // Build master's capabilities (same logic as WorkerModuleExecutor)
        var options = _options.Value;
        var capabilities = new HashSet<Capability>(options.Capabilities);
        if (options.AutoDetectOsCapability)
        {
            capabilities.UnionWith(OsCapabilityDetector.Detect());
        }

        _logger.LogInformation("Master worker loop started with capabilities: {Capabilities}",
            string.Join(", ", capabilities));

        using var workerScheduler = new WorkerModuleScheduler();

        while (!workerCancellationToken.IsCancellationRequested)
        {
            try
            {
                var assignment = await _workerCoordinator
                    .DequeueModuleAsync(capabilities, workerCancellationToken)
                    .ConfigureAwait(false);
                if (assignment is null)
                {
                    break;
                }

                if (pipelineCancellationToken.IsCancellationRequested && !assignment.Configuration.AlwaysRun)
                {
                    _logger.LogInformation(
                        "Master skipping cancelled module {Module}",
                        assignment.ModuleTypeName);
                    continue;
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
                    workerScheduler,
                    executionCancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Master worker loop encountered an error");
            }
        }
    }

    private async Task ExecuteAssignmentAsync(
        ModuleAssignment assignment,
        IReadOnlyList<IModule> modules,
        Dictionary<string, IModule> moduleLookup,
        WorkerModuleScheduler workerScheduler,
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
            await ExecuteAndPublishAsync(assignment, module, workerScheduler, cancellationToken);
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
        WorkerModuleScheduler workerScheduler,
        CancellationToken cancellationToken)
    {
        var moduleType = module.GetType();
        await using var serviceScope = _serviceScopeFactory.CreateAsyncScope();
        var moduleLogger = serviceScope.ServiceProvider
            .GetRequiredService<IInternalModuleLoggerAccessor>()
            .GetLogger(moduleType) as IInternalModuleLogger
            ?? throw new InvalidOperationException($"No internal module logger is available for {moduleType.Name}.");
        await using var loggerScope = new ModuleLoggerScope(moduleLogger, moduleType);

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
                    workerScheduler,
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
        Action requestFailureCancellation)
    {
        var pipelineToken = module.Configuration.AlwaysRun
            ? _lifetime.ApplicationStopping
            : cts.Token;
        using var timeoutCts = CreateResultTimeoutSource(module.Configuration.Timeout, pipelineToken);
        var lifecycleToken = timeoutCts?.Token ?? pipelineToken;

        try
        {
            _logger.LogInformation("Distributing module {Module} to workers", moduleType.Name);
            await _publisher.PublishAsync(assignment, lifecycleToken);
            await CollectResultAsync(
                module,
                moduleType,
                scheduler,
                cts,
                requestFailureCancellation,
                lifecycleToken);
        }
        catch (OperationCanceledException) when (!cts.IsCancellationRequested)
        {
            // Timeout expired (not pipeline cancellation)
            _logger.LogError("Distributed module {Module} timed out waiting for result — worker may have died", moduleType.Name);
            RegisterFailureResult(
                module,
                moduleType,
                new TimeoutException(
                    $"Module {moduleType.Name} did not produce a result within the configured timeout"),
                ModuleStatus.TimedOut);
            scheduler.MarkModuleCompleted(moduleType, false);
            requestFailureCancellation();
            await cts.CancelAsync();
        }
        catch (OperationCanceledException exception)
        {
            _resultRegistrar.RegisterTerminatedResult(module, moduleType, exception);
            scheduler.MarkModuleCompleted(moduleType, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish or collect distributed module {Module}", moduleType.Name);
            RegisterFailureResult(module, moduleType, ex, ModuleStatus.Failed);
            scheduler.MarkModuleCompleted(moduleType, false, ex);
            requestFailureCancellation();
            await cts.CancelAsync();
        }
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
        Action requestFailureCancellation,
        CancellationToken cancellationToken)
    {
        var result = await _resultCollector.WaitForResultAsync(moduleType.FullName!, cancellationToken);
        var success = result is not null && result.ExceptionOrDefault is null;

        if (result is not null)
        {
            ModuleCompletionSourceApplicator.TryApply(module, result);
            _resultRegistry.RegisterResult(moduleType, result);
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
        ModuleStatus status)
    {
        try
        {
            var failureResult = CreateCollectorFailureResult(
                module,
                moduleType,
                exception,
                status);
            ModuleCompletionSourceApplicator.TryApply(module, failureResult);
            _resultRegistry.RegisterResult(moduleType, failureResult);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to register failure result for module {Module}", moduleType.Name);
        }
    }
}
