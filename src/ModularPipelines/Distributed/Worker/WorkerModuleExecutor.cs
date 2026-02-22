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
            RegisteredAt: DateTimeOffset.UtcNow,
            Status: WorkerStatus.Connected,
            CurrentModule: null
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
                    _logger.LogError("Cannot resolve module type: {ModuleTypeName}", assignment.ModuleTypeName);
                    continue;
                }

                // Find the module instance from the registered modules
                var module = modules.FirstOrDefault(m => m.GetType().FullName == assignment.ModuleTypeName);
                if (module is null)
                {
                    _logger.LogError("Module instance not found: {ModuleTypeName}", assignment.ModuleTypeName);
                    continue;
                }

                // Download consumed artifacts before execution
                if (_artifactLifecycleManager is not null)
                {
                    try
                    {
                        await _artifactLifecycleManager.DownloadConsumedArtifactsAsync(module.GetType(), cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to download artifacts for module {Module}", assignment.ModuleTypeName);
                    }
                }

                // Execute the module through the framework's execution pipeline
                try
                {
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

                    // Publish failure result
                    var failureResult = ModuleResult.CreateFailure(
                        ex,
                        new ModuleExecutionContext(module, module.GetType()));
                    var serialized = _serializer.Serialize(
                        failureResult,
                        assignment.ModuleTypeName,
                        assignment.ResultTypeName,
                        options.InstanceIndex);
                    await _coordinator.PublishResultAsync(serialized, cancellationToken);
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
}
