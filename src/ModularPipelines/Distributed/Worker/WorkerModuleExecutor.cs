using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModuleResultFactory = ModularPipelines.Engine.Execution.ModuleResultFactory;

namespace ModularPipelines.Distributed.Worker;

internal class WorkerModuleExecutor(
    IHostApplicationLifetime lifetime,
    IDistributedCoordinator coordinator,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleRunner moduleRunner,
    IOptions<DistributedOptions> options,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<WorkerModuleExecutor> logger) : IModuleExecutor
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly ArtifactLifecycleManager? _artifactLifecycleManager = artifactLifecycleManager;
    private readonly ILogger<WorkerModuleExecutor> _logger = logger;

    public async Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        var options = _options.Value;
        var cancellationToken = _lifetime.ApplicationStopping;

        // Register all module types for deserialization
        foreach (var module in modules)
        {
            _typeRegistry.Register(module.GetType());
        }

        // Build capabilities
        var capabilities = new HashSet<string>(options.Capabilities, StringComparer.OrdinalIgnoreCase);
        if (options.AutoDetectOsCapability)
        {
            var osCapability = OsCapabilityDetector.Detect();
            if (osCapability is not null)
            {
                capabilities.Add(osCapability);
            }
        }

        // Register with coordinator
        var registration = new WorkerRegistration(
            WorkerIndex: options.InstanceIndex,
            Capabilities: capabilities,
            RegisteredAt: DateTimeOffset.UtcNow
        );
        await _coordinator.RegisterWorkerAsync(registration, cancellationToken);
        _logger.LogInformation("Worker {Index} registered with capabilities: {Capabilities}",
            options.InstanceIndex, string.Join(", ", capabilities));

        var executedModules = new List<IModule>();
        using var workerScheduler = new WorkerModuleScheduler();

        // Worker execution loop
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var assignment = await _coordinator.DequeueModuleAsync(capabilities, cancellationToken);
                if (assignment is null)
                {
                    // No more work available
                    break;
                }

                _logger.LogInformation("Worker {Index} executing module {Module}",
                    options.InstanceIndex, assignment.ModuleTypeName);

                var resolved = _typeRegistry.Resolve(assignment.ModuleTypeName);
                if (resolved is null)
                {
                    _logger.LogError("Cannot resolve module type: {ModuleTypeName}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
                    await PublishResolutionFailureAsync(assignment, cancellationToken);
                    continue;
                }

                // Find the module instance from the registered modules
                var module = modules.FirstOrDefault(m => m.GetType().FullName == assignment.ModuleTypeName);
                if (module is null)
                {
                    _logger.LogError("Module instance not found: {ModuleTypeName}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
                    await PublishResolutionFailureAsync(assignment, cancellationToken);
                    continue;
                }

                // Apply dependency results so that GetModule<T>() works cross-process
                if (assignment.DependencyResults is { Count: > 0 })
                {
                    ApplyDependencyResults(assignment.DependencyResults, modules);
                }

                // Execute the module through the framework's execution pipeline
                try
                {
                    // Download consumed artifacts before execution
                    if (_artifactLifecycleManager is not null)
                    {
                        await _artifactLifecycleManager.DownloadConsumedArtifactsAsync(module.GetType(), cancellationToken);
                    }

                    var moduleState = new ModuleState(module, module.GetType());
                    await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, workerScheduler, cancellationToken);

                    // CompletionSource is set by ModuleExecutionPipeline — get the result
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
                            _logger.LogError(ex, "Failed to upload artifacts for module {Module}", assignment.ModuleTypeName);
                        }
                    }

                    if (result is not null)
                    {
                        var serialized = _serializer.Serialize(
                            result,
                            assignment.ModuleTypeName,
                            assignment.ResultTypeName,
                            options.InstanceIndex);

                        if (artifactRefs is not null)
                        {
                            serialized = serialized with { Artifacts = artifactRefs };
                        }

                        await _coordinator.PublishResultAsync(serialized, cancellationToken);
                    }

                    executedModules.Add(module);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Module {Module} execution failed on worker {Index}",
                        assignment.ModuleTypeName, options.InstanceIndex);

                    // Publish failure result — wrapped in try/catch to prevent master deadlock
                    try
                    {
                        var failureResult = ModuleResultFactory.CreateException(
                            resolved.Value.ResultType,
                            ex,
                            new ModuleExecutionContext(module, module.GetType()));
                        var serialized = _serializer.Serialize(
                            failureResult,
                            assignment.ModuleTypeName,
                            assignment.ResultTypeName,
                            options.InstanceIndex);
                        await _coordinator.PublishResultAsync(serialized, cancellationToken);
                    }
                    catch (Exception publishEx)
                    {
                        _logger.LogCritical(publishEx,
                            "Failed to publish failure result for module {Module} — master may hang waiting for this result",
                            assignment.ModuleTypeName);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Worker {Index} shutting down", options.InstanceIndex);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Worker {Index} encountered an error in execution loop", options.InstanceIndex);
            }
        }

        return executedModules;
    }

    private async Task PublishResolutionFailureAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        try
        {
            var failureResult = new SerializedModuleResult(
                ModuleTypeName: assignment.ModuleTypeName,
                ResultTypeName: assignment.ResultTypeName,
                WorkerIndex: _options.Value.InstanceIndex,
                SerializedJson: "null",
                CompletedAt: DateTimeOffset.UtcNow);
            await _coordinator.PublishResultAsync(failureResult, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex,
                "Failed to publish resolution failure for {Module} — master may hang waiting for this result",
                assignment.ModuleTypeName);
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
                if (serializedDep.SerializedJson.StartsWith(Master.DistributedWorkPublisher.GzipPrefix, StringComparison.Ordinal))
                {
                    var decompressed = Master.DistributedWorkPublisher.DecompressJson(serializedDep.SerializedJson);
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
}
