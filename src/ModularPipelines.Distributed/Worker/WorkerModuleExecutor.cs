using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Worker;

internal class WorkerModuleExecutor(
    IServiceProvider serviceProvider,
    IDistributedCoordinator coordinator,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IOptions<DistributedOptions> options,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<WorkerModuleExecutor> logger) : IModuleExecutor
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly ArtifactLifecycleManager? _artifactLifecycleManager = artifactLifecycleManager;
    private readonly ILogger<WorkerModuleExecutor> _logger = logger;

    public async Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        var options = _options.Value;

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
        await _coordinator.RegisterWorkerAsync(registration, CancellationToken.None);
        _logger.LogInformation("Worker {Index} registered with capabilities: {Capabilities}",
            options.InstanceIndex, string.Join(", ", capabilities));

        var executedModules = new List<IModule>();

        // Worker execution loop
        while (true)
        {
            try
            {
                var assignment = await _coordinator.DequeueModuleAsync(capabilities, CancellationToken.None);
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
                        await _artifactLifecycleManager.DownloadConsumedArtifactsAsync(module.GetType(), CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to download artifacts for module {Module}", assignment.ModuleTypeName);
                    }
                }

                // Execute the module
                try
                {
                    // Access the internal CompletionSource.Task via reflection to await the module result.
                    // CompletionSource is internal but accessible via InternalsVisibleTo.
                    // The actual execution is triggered through the normal pipeline;
                    // this placeholder awaits completion and retrieves the result.
                    var completionSourceProp = module.GetType().GetProperty(
                        "CompletionSource",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!;
                    var completionSource = completionSourceProp.GetValue(module)!;
                    var taskProp = completionSource.GetType().GetProperty("Task")!;
                    var task = (Task) taskProp.GetValue(completionSource)!;
                    await task;

                    // Get the Result property from the completed task
                    var resultProp = task.GetType().GetProperty("Result")!;
                    var result = resultProp.GetValue(task) as IModuleResult;

                    // Upload produced artifacts before publishing result
                    IReadOnlyList<ArtifactReference>? artifactRefs = null;
                    if (_artifactLifecycleManager is not null)
                    {
                        try
                        {
                            artifactRefs = await _artifactLifecycleManager.UploadProducedArtifactsAsync(module.GetType(), CancellationToken.None);
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

                        await _coordinator.PublishResultAsync(serialized, CancellationToken.None);
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
                    await _coordinator.PublishResultAsync(serialized, CancellationToken.None);
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
