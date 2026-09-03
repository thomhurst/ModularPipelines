using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModuleResultFactory = ModularPipelines.Engine.Execution.ModuleResultFactory;

namespace ModularPipelines.Distributed.Worker;

internal class WorkerModuleExecutor(
    IHostApplicationLifetime lifetime,
    IDistributedWorkerCoordinator coordinator,
    IEnumerable<IModule> registeredModules,
    ModuleTypeRegistry typeRegistry,
    ModuleResultSerializer serializer,
    IModuleRunner moduleRunner,
    IModuleResultRegistry resultRegistry,
    IModuleDependencyRegistry dependencyRegistry,
    IModuleMetadataRegistry metadataRegistry,
    IOptions<DistributedOptions> options,
    IServiceScopeFactory serviceScopeFactory,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<WorkerModuleExecutor> logger,
    DistributedConditionRouting? conditionRouting = null) : IModuleExecutor
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IDistributedWorkerCoordinator _coordinator = coordinator;
    private readonly IReadOnlyList<IModule> _registeredModules = registeredModules
        .Distinct<IModule>(ReferenceEqualityComparer.Instance)
        .ToArray();

    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleDependencyRegistry _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry = metadataRegistry;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ArtifactLifecycleManager? _artifactLifecycleManager = artifactLifecycleManager;
    private readonly ILogger<WorkerModuleExecutor> _logger = logger;
    private readonly DistributedConditionRouting? _conditionRouting = conditionRouting;

    public async Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        var options = _options.Value;
        using var executionCts = CancellationTokenSource.CreateLinkedTokenSource(
            _lifetime.ApplicationStopping);
        var cancellationToken = executionCts.Token;
        var availableModules = _registeredModules
            .Concat(modules)
            .Distinct<IModule>(ReferenceEqualityComparer.Instance)
            .ToArray();

        foreach (var module in availableModules)
        {
            _typeRegistry.Register(module.GetType());
        }

        var moduleLookup = DependencyResultApplicator.BuildModuleLookup(availableModules);
        var capabilities = BuildCapabilities(options);
        await RegisterWorkerAsync(options.InstanceIndex, capabilities, cancellationToken);
        var heartbeatTask = SendHeartbeatsAsync(
            options.InstanceIndex,
            options.WorkerHeartbeatInterval,
            cancellationToken);
        var cancellationTask = ObserveDistributedCancellationAsync(
            executionCts,
            options.WorkerHeartbeatInterval);

        var executedModules = new List<IModule>();
        using var workerScheduler = new WorkerModuleScheduler();

        try
        {
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
        }
        finally
        {
            await executionCts.CancelAsync();
            await AwaitBackgroundTasksAsync(heartbeatTask, cancellationTask);
        }

        return executedModules;
    }

    private async Task SendHeartbeatsAsync(
        int workerIndex,
        TimeSpan interval,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(interval, cancellationToken);
                await _coordinator.SendHeartbeatAsync(workerIndex, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Worker {Index} heartbeat failed", workerIndex);
            }
        }
    }

    private async Task ObserveDistributedCancellationAsync(
        CancellationTokenSource executionCts,
        TimeSpan retryInterval)
    {
        while (!executionCts.IsCancellationRequested)
        {
            try
            {
                await _coordinator.WaitForCancellationAsync(executionCts.Token);
                if (!executionCts.IsCancellationRequested)
                {
                    _logger.LogInformation("Master requested distributed cancellation");
                    await executionCts.CancelAsync();
                }

                return;
            }
            catch (OperationCanceledException) when (executionCts.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Worker cancellation observer failed; retrying");
            }

            try
            {
                await Task.Delay(retryInterval, executionCts.Token);
            }
            catch (OperationCanceledException) when (executionCts.IsCancellationRequested)
            {
                return;
            }
        }
    }

    private static async Task AwaitBackgroundTasksAsync(params Task[] tasks)
    {
        try
        {
            await Task.WhenAll(tasks);
        }
        catch (OperationCanceledException)
        {
        }
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

    private async Task RegisterWorkerAsync(int instanceIndex, HashSet<Capability> capabilities, CancellationToken cancellationToken)
    {
        var registration = new WorkerRegistration(
            WorkerIndex: instanceIndex,
            Capabilities: capabilities,
            RegisteredAt: DateTimeOffset.UtcNow)
        {
            RunId = _options.Value.RunId,
        };
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
            DependencyResultApplicator.Apply(
                assignment.DependencyResults,
                moduleLookup,
                _serializer,
                _resultRegistry,
                _logger);
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
            _conditionRouting?.RestoreLocallySatisfiedGroups(
                module,
                assignment.SatisfiedConditionGroups);
            ModuleStateDependencyInitializer.Populate(
                moduleState,
                _typeRegistry.GetRegisteredModuleTypes(),
                _dependencyRegistry,
                _metadataRegistry);
            await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, workerScheduler, cancellationToken);

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
                instanceIndex);
            if (artifactReferences is not null)
            {
                serialized = serialized with { Artifacts = artifactReferences };
            }

            await _coordinator.PublishResultAsync(serialized, cancellationToken);
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
            moduleLogger.LogError(ex, "Failed to upload artifacts for module {Module}", moduleTypeName);
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
