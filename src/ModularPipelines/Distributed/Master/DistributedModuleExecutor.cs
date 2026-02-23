using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Master;

internal class DistributedModuleExecutor(
    IHostApplicationLifetime lifetime,
    IModuleSchedulerFactory schedulerFactory,
    IModuleRunner moduleRunner,
    IRegistrationEventExecutor registrationEventExecutor,
    IDistributedCoordinator coordinator,
    DistributedWorkPublisher publisher,
    DistributedResultCollector resultCollector,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleResultRegistry resultRegistry,
    IOptions<DistributedOptions> options,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<DistributedModuleExecutor> logger) : IModuleExecutor
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IModuleSchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IRegistrationEventExecutor _registrationEventExecutor = registrationEventExecutor;
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly DistributedWorkPublisher _publisher = publisher;
    private readonly DistributedResultCollector _resultCollector = resultCollector;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IOptions<DistributedOptions> _options = options;
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

        // Invoke registration events before dependency resolution
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(modules).ConfigureAwait(false);

        // Wait for workers to register before distributing work
        await WaitForWorkersAsync(_lifetime.ApplicationStopping);

        IModuleScheduler? scheduler = null;
        try
        {
            scheduler = _schedulerFactory.Create();
            scheduler.InitializeModules(modules);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.ApplicationStopping);
            cts.Token.Register(() => scheduler.CancelPendingModules());

            var schedulerTask = scheduler.RunSchedulerAsync(cts.Token);
            var resultTasks = new List<Task>();

            // Start the master worker loop — the master participates as a worker,
            // dequeuing and executing modules from the same queue as external workers.
            var masterWorkerTask = RunMasterWorkerLoopAsync(modules, cts.Token);

            try
            {
                await foreach (var moduleState in scheduler.ReadyModules.ReadAllAsync(cts.Token))
                {
                    var moduleType = moduleState.Module.GetType();

                    // TODO(matrix): MatrixModuleExpander.ScanForExpansions not yet connected.
                    // Modules with [MatrixTarget] will run once, not N times.
                    _logger.LogInformation("Distributing module {Module} to workers", moduleType.Name);
                    var assignment = _publisher.CreateAssignment(moduleState.Module);
                    await _publisher.PublishAsync(assignment, cts.Token);

                    var collectTask = CollectDistributedResultAsync(moduleState.Module, moduleType, scheduler, cts);
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

            // All results collected — cancel to stop the master worker loop
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
        finally
        {
            // Always signal workers to stop — whether the master succeeded or crashed.
            // Without this, workers hang forever waiting for work that will never come.
            try
            {
                await _coordinator.SignalCompletionAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to signal completion to workers during shutdown");
            }

            scheduler?.Dispose();
        }

        return modules;
    }

    private async Task WaitForWorkersAsync(CancellationToken cancellationToken)
    {
        var expectedWorkers = _options.Value.TotalInstances - 1;
        if (expectedWorkers <= 0)
        {
            return;
        }

        var timeout = TimeSpan.FromSeconds(_options.Value.CapabilityTimeoutSeconds);
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(timeout);

        _logger.LogInformation("Waiting for {Expected} worker(s) to register (timeout: {Timeout}s)...",
            expectedWorkers, _options.Value.CapabilityTimeoutSeconds);

        var lastCount = 0;
        while (!timeoutCts.IsCancellationRequested)
        {
            try
            {
                var workers = await _coordinator.GetRegisteredWorkersAsync(timeoutCts.Token);
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
                    "Worker registration timeout ({Timeout}s expired). {Count}/{Expected} worker(s) registered — proceeding with available workers",
                    _options.Value.CapabilityTimeoutSeconds, lastCount, expectedWorkers);
                return;
            }
        }
    }

    private async Task RunMasterWorkerLoopAsync(IReadOnlyList<IModule> modules, CancellationToken cancellationToken)
    {
        // Build master's capabilities (same logic as WorkerModuleExecutor)
        var options = _options.Value;
        var capabilities = new HashSet<string>(options.Capabilities, StringComparer.OrdinalIgnoreCase);
        if (options.AutoDetectOsCapability)
        {
            var osCapability = OsCapabilityDetector.Detect();
            if (osCapability is not null)
            {
                capabilities.Add(osCapability);
            }
        }

        _logger.LogInformation("Master worker loop started with capabilities: {Capabilities}",
            string.Join(", ", capabilities));

        using var workerScheduler = new WorkerModuleScheduler();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var assignment = await _coordinator.DequeueModuleAsync(capabilities, cancellationToken);
                if (assignment is null)
                {
                    break;
                }

                _logger.LogInformation("Master executing module {Module} locally",
                    assignment.ModuleTypeName);

                await ExecuteAssignmentAsync(assignment, modules, workerScheduler, cancellationToken);
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
        WorkerModuleScheduler workerScheduler,
        CancellationToken cancellationToken)
    {
        var resolved = _typeRegistry.Resolve(assignment.ModuleTypeName);
        if (resolved is null)
        {
            _logger.LogError("Cannot resolve module type: {Type}", assignment.ModuleTypeName);
            return;
        }

        var module = modules.FirstOrDefault(m => m.GetType().FullName == assignment.ModuleTypeName);
        if (module is null)
        {
            _logger.LogError("Module instance not found: {Type}", assignment.ModuleTypeName);
            return;
        }

        // Apply dependency results so that GetModule<T>() works
        if (assignment.DependencyResults is { Count: > 0 })
        {
            ApplyDependencyResults(assignment.DependencyResults, modules);
        }

        try
        {
            // Download consumed artifacts before execution
            if (_artifactLifecycleManager is not null)
            {
                await _artifactLifecycleManager.DownloadConsumedArtifactsAsync(module.GetType(), cancellationToken);
            }

            var moduleState = new ModuleState(module, module.GetType());
            await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, workerScheduler, cancellationToken);

            var result = await module.ResultTask;

            // Upload produced artifacts before publishing result
            IReadOnlyList<ArtifactReference>? artifactRefs = null;
            if (_artifactLifecycleManager is not null)
            {
                try
                {
                    artifactRefs = await _artifactLifecycleManager.UploadProducedArtifactsAsync(module.GetType(), cancellationToken);
                    if (artifactRefs.Count == 0)
                    {
                        artifactRefs = null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to upload artifacts for {Module}", assignment.ModuleTypeName);
                }
            }

            if (result is not null)
            {
                var serialized = _serializer.Serialize(result, assignment.ModuleTypeName, assignment.ResultTypeName, _options.Value.InstanceIndex);
                if (artifactRefs is not null)
                {
                    serialized = serialized with { Artifacts = artifactRefs };
                }

                await _coordinator.PublishResultAsync(serialized, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Module {Module} execution failed on master", assignment.ModuleTypeName);

            // Publish failure result so the collector doesn't deadlock
            try
            {
                var failureResult = ModuleResultFactory.CreateException(
                    resolved.Value.ResultType,
                    ex,
                    new ModuleExecutionContext(module, module.GetType()));
                var serialized = _serializer.Serialize(failureResult, assignment.ModuleTypeName, assignment.ResultTypeName, _options.Value.InstanceIndex);
                await _coordinator.PublishResultAsync(serialized, cancellationToken);
            }
            catch (Exception publishEx)
            {
                _logger.LogCritical(publishEx, "Failed to publish failure result for {Module}", assignment.ModuleTypeName);
            }
        }
    }

    /// <summary>
    /// Applies dependency results received in the assignment to local module instances.
    /// This enables <c>GetModule&lt;T&gt;()</c> to resolve cross-process dependencies.
    /// <c>TrySetResult</c> is idempotent — safe if CompletionSource was already set.
    /// </summary>
    private void ApplyDependencyResults(IReadOnlyList<SerializedModuleResult> dependencyResults, IReadOnlyList<IModule> modules)
    {
        foreach (var serializedDep in dependencyResults)
        {
            var depModule = modules.FirstOrDefault(m => m.GetType().FullName == serializedDep.ModuleTypeName);
            if (depModule is null)
            {
                _logger.LogDebug("Dependency module instance not found locally: {ModuleTypeName}", serializedDep.ModuleTypeName);
                continue;
            }

            try
            {
                // Decompress GZip-compressed dependency results before deserialization
                var toDeserialize = serializedDep;
                if (serializedDep.SerializedJson.StartsWith(DistributedWorkPublisher.GzipPrefix, StringComparison.Ordinal))
                {
                    var decompressed = DistributedWorkPublisher.DecompressJson(serializedDep.SerializedJson);
                    toDeserialize = serializedDep with { SerializedJson = decompressed };
                }

                var result = _serializer.Deserialize(toDeserialize);
                if (result is not null)
                {
                    ModuleCompletionSourceApplicator.TryApply(depModule, result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to apply dependency result for {ModuleTypeName}", serializedDep.ModuleTypeName);
            }
        }
    }

    private async Task CollectDistributedResultAsync(IModule module, Type moduleType, IModuleScheduler scheduler, CancellationTokenSource cts)
    {
        try
        {
            scheduler.MarkModuleStarted(moduleType);
            var result = await _resultCollector.WaitForResultAsync(moduleType.FullName!, cts.Token);
            var success = result is not null && !result.IsFailure;

            // Apply the deserialized result to the module's CompletionSource so that
            // DependsOn<T> result access works across the master/worker boundary
            if (result is not null)
            {
                ModuleCompletionSourceApplicator.TryApply(module, result);
                _resultRegistry.RegisterResult(moduleType, result);
            }

            scheduler.MarkModuleCompleted(moduleType, success);

            if (!success)
            {
                _logger.LogError("Distributed module {Module} failed on worker — cancelling pipeline", moduleType.Name);
                await cts.CancelAsync();
            }
        }
        catch (OperationCanceledException)
        {
            RegisterFailureResult(module, moduleType, new OperationCanceledException("Module was cancelled"));
            scheduler.MarkModuleCompleted(moduleType, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to collect result for distributed module {Module}", moduleType.Name);
            RegisterFailureResult(module, moduleType, ex);
            scheduler.MarkModuleCompleted(moduleType, false, ex);
            await cts.CancelAsync();
        }
    }

    private void RegisterFailureResult(IModule module, Type moduleType, Exception exception)
    {
        try
        {
            var failureResult = ModuleResultFactory.CreateException(
                module.ResultType,
                exception,
                new ModuleExecutionContext(module, moduleType));
            _resultRegistry.RegisterResult(moduleType, failureResult);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to register failure result for module {Module}", moduleType.Name);
        }
    }

}
