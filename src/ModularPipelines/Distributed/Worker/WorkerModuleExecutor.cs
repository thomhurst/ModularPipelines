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

        foreach (var module in modules)
        {
            _typeRegistry.Register(module.GetType());
        }

        var moduleLookup = DependencyResultApplicator.BuildModuleLookup(modules);
        var capabilities = BuildCapabilities(options);
        await RegisterWorkerAsync(options.InstanceIndex, capabilities, cancellationToken);

        var executedModules = new List<IModule>();
        using var workerScheduler = new WorkerModuleScheduler();

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
                await ExecuteAssignmentAsync(
                    assignment,
                    moduleLookup,
                    workerScheduler,
                    executedModules,
                    options.InstanceIndex,
                    cancellationToken);
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

    private static HashSet<string> BuildCapabilities(DistributedOptions options)
    {
        var capabilities = new HashSet<string>(options.Capabilities, StringComparer.OrdinalIgnoreCase);
        if (options.AutoDetectOsCapability && OsCapabilityDetector.Detect() is { } osCapability)
        {
            capabilities.Add(osCapability);
        }

        return capabilities;
    }

    private async Task RegisterWorkerAsync(int instanceIndex, HashSet<string> capabilities, CancellationToken cancellationToken)
    {
        var registration = new WorkerRegistration(
            WorkerIndex: instanceIndex,
            Capabilities: capabilities,
            RegisteredAt: DateTimeOffset.UtcNow);
        await _coordinator.RegisterWorkerAsync(registration, cancellationToken);
        _logger.LogInformation("Worker {Index} registered with capabilities: {Capabilities}",
            instanceIndex, string.Join(", ", capabilities));
    }

    private async Task ExecuteAssignmentAsync(
        ModuleAssignment assignment,
        Dictionary<string, IModule> moduleLookup,
        WorkerModuleScheduler workerScheduler,
        List<IModule> executedModules,
        int instanceIndex,
        CancellationToken cancellationToken)
    {
        var resolved = _typeRegistry.Resolve(assignment.ModuleTypeName);
        if (resolved is null)
        {
            _logger.LogError("Cannot resolve module type: {ModuleTypeName}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
            await DependencyResultApplicator.PublishResolutionFailureAsync(assignment, instanceIndex, _coordinator, _logger, cancellationToken);
            return;
        }

        if (!moduleLookup.TryGetValue(assignment.ModuleTypeName, out var module))
        {
            _logger.LogError("Module instance not found: {ModuleTypeName}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
            await DependencyResultApplicator.PublishResolutionFailureAsync(assignment, instanceIndex, _coordinator, _logger, cancellationToken);
            return;
        }

        if (assignment.DependencyResults is { Count: > 0 })
        {
            DependencyResultApplicator.Apply(assignment.DependencyResults, moduleLookup, _serializer, _logger);
        }

        try
        {
            await ExecuteAndPublishAsync(assignment, module, workerScheduler, instanceIndex, cancellationToken);
            executedModules.Add(module);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Module {Module} execution failed on worker {Index}",
                assignment.ModuleTypeName, instanceIndex);
            await PublishFailureAsync(assignment, resolved.Value.ResultType, module, ex, instanceIndex, cancellationToken);
        }
    }

    private async Task ExecuteAndPublishAsync(
        ModuleAssignment assignment,
        IModule module,
        WorkerModuleScheduler workerScheduler,
        int instanceIndex,
        CancellationToken cancellationToken)
    {
        if (_artifactLifecycleManager is not null)
        {
            await _artifactLifecycleManager.DownloadConsumedArtifactsAsync(module.GetType(), cancellationToken);
        }

        var moduleState = new ModuleState(module, module.GetType());
        await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, workerScheduler, cancellationToken);

        var result = await module.ResultTask;
        var artifactReferences = await TryUploadArtifactsAsync(module, assignment.ModuleTypeName, cancellationToken);
        if (result is null)
        {
            return;
        }

        var serialized = _serializer.Serialize(
            result,
            assignment.ModuleTypeName,
            assignment.ResultTypeName,
            instanceIndex);
        if (artifactReferences is not null)
        {
            serialized = serialized with { Artifacts = artifactReferences };
        }

        await _coordinator.PublishResultAsync(serialized, cancellationToken);
    }

    private async Task<IReadOnlyList<ArtifactReference>?> TryUploadArtifactsAsync(
        IModule module,
        string moduleTypeName,
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
            _logger.LogError(ex, "Failed to upload artifacts for module {Module}", moduleTypeName);
            return null;
        }
    }

    private async Task PublishFailureAsync(
        ModuleAssignment assignment,
        Type resultType,
        IModule module,
        Exception exception,
        int instanceIndex,
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
                instanceIndex);
            await _coordinator.PublishResultAsync(serialized, cancellationToken);
        }
        catch (Exception publishException)
        {
            _logger.LogCritical(publishException,
                "Failed to publish failure result for module {Module} — master may hang waiting for this result",
                assignment.ModuleTypeName);
        }
    }
}
