using System.Collections.Concurrent;
using System.Diagnostics;
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
using ModularPipelines.Helpers;
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
    IParallelLimitProvider parallelLimitProvider,
    IServiceScopeFactory serviceScopeFactory,
    ArtifactLifecycleManager? artifactLifecycleManager,
    ILogger<WorkerModuleExecutor> logger,
    IExecutionLocationContext? executionLocationContext = null) : IExecutionBackend
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IDistributedWorkerCoordinator _coordinator = coordinator;
    private readonly IReadOnlyList<IModule> _registeredModules = [.. registeredModules.Distinct<IModule>(ReferenceEqualityComparer.Instance)];

    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ModuleResultSerializer _serializer = serializer;
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IModuleDependencyRegistry _dependencyRegistry = dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry = metadataRegistry;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly IParallelLimitProvider _parallelLimitProvider = parallelLimitProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ArtifactLifecycleManager? _artifactLifecycleManager = artifactLifecycleManager;
    private readonly ILogger<WorkerModuleExecutor> _logger = logger;
    private readonly IExecutionLocationContext? _executionLocationContext = executionLocationContext;

    public bool OwnsEntirePlan => false;

    // Workers execute whatever the master assigns, so the plan's duration estimates only
    // influence the master's scheduling and are not consulted here.
    public async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
        IExecutionBackendContext context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(estimatedDurations);
        ArgumentNullException.ThrowIfNull(context);

        var options = _options.Value;
        using var executionCts = CancellationTokenSource.CreateLinkedTokenSource(
            _lifetime.ApplicationStopping,
            cancellationToken);
        cancellationToken = executionCts.Token;
        var availableModules = _registeredModules
            .Concat(modules)
            .Distinct<IModule>(ReferenceEqualityComparer.Instance)
            .ToArray();

        foreach (var module in availableModules)
        {
            _typeRegistry.Register(module.GetType());
        }

        var moduleLookup = DependencyResultApplicator.BuildModuleLookup(availableModules);
        var dependencyResultCache = new DependencyResultCache(_coordinator, cancellationToken);
        var capabilities = BuildCapabilities(options);
        var maxConcurrency = DistributedWorkerPool.GetMaxConcurrency(
            _parallelLimitProvider,
            options);
        await RegisterWorkerAsync(options.InstanceIndex, capabilities, cancellationToken);
        var heartbeatTask = SendHeartbeatsAsync(
            options.InstanceIndex,
            options.RunId,
            options.WorkerHeartbeatInterval,
            cancellationToken);
        var cancellationTask = ObserveDistributedCancellationAsync(
            executionCts,
            options.WorkerHeartbeatInterval);

        var executedModules = new ConcurrentQueue<IModule>();
        try
        {
            _logger.LogInformation(
                "Worker {Index} starting {MaxConcurrency} concurrent execution slot(s)",
                options.InstanceIndex,
                maxConcurrency);
            await DistributedWorkerPool.RunAsync(
                token => _coordinator.DequeueModuleAsync(capabilities, token),
                async (assignment, claimedAt, token) =>
                {
                    _logger.LogInformation("Worker {Index} executing module {Module}",
                        options.InstanceIndex, assignment.ModuleTypeName);
                    await ExecuteAssignmentAsync(
                        assignment,
                        claimedAt,
                        moduleLookup,
                        dependencyResultCache,
                        executedModules,
                        options.InstanceIndex,
                        token).ConfigureAwait(false);
                },
                maxConcurrency,
                exception => _logger.LogError(
                    exception,
                    "Worker {Index} encountered an error in execution loop",
                    options.InstanceIndex),
                cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            await executionCts.CancelAsync();
            await AwaitBackgroundTasksAsync(heartbeatTask, cancellationTask);
        }

        return _resultRegistry.GetCompletedResults(executedModules);
    }

    internal Task<IReadOnlyList<IModuleResult>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        return ExecuteAsync(
            modules,
            new Dictionary<Type, TimeSpan>(),
            new ExecutionBackendContext(_resultRegistry),
            CancellationToken.None);
    }

    private async Task SendHeartbeatsAsync(
        int workerIndex,
        string? runIdentifier,
        TimeSpan interval,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(interval, cancellationToken);
                await _coordinator.SendHeartbeatAsync(
                        new WorkerStatus(workerIndex)
                        {
                            RunId = runIdentifier,
                        },
                        cancellationToken)
                    .ConfigureAwait(false);
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
            Capabilities: [.. capabilities],
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
        DateTimeOffset claimedAt,
        Dictionary<string, IModule> moduleLookup,
        DependencyResultCache dependencyResultCache,
        ConcurrentQueue<IModule> executedModules,
        int instanceIndex,
        CancellationToken cancellationToken)
    {
        var executionTimer = new DistributedModuleExecutionTimer(claimedAt);
        var resolved = _typeRegistry.Resolve(assignment.ModuleTypeName);
        if (resolved is null)
        {
            _logger.LogError("Cannot resolve module type: {ModuleTypeName}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
            await DependencyResultApplicator.PublishResolutionFailureAsync(assignment, instanceIndex, _coordinator, _logger, executionTimer).ConfigureAwait(false);
            return;
        }

        if (!moduleLookup.TryGetValue(assignment.ModuleTypeName, out var module))
        {
            _logger.LogError("Module instance not found: {ModuleTypeName}. Publishing failure to prevent master hang.", assignment.ModuleTypeName);
            await DependencyResultApplicator.PublishResolutionFailureAsync(assignment, instanceIndex, _coordinator, _logger, executionTimer).ConfigureAwait(false);
            return;
        }

        try
        {
            if (assignment.DependencyResultReferences is { Count: > 0 })
            {
                await DependencyResultApplicator.FetchAndApplyAsync(
                    assignment.DependencyResultReferences,
                    dependencyResultCache,
                    moduleLookup,
                    _serializer,
                    _resultRegistry,
                    _logger,
                    executionTimer).ConfigureAwait(false);
            }

            await ExecuteAndPublishAsync(assignment, module, instanceIndex, executionTimer, cancellationToken).ConfigureAwait(false);
            executedModules.Enqueue(module);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Module {Module} execution failed on worker {Index}",
                assignment.ModuleTypeName, instanceIndex);
            await PublishFailureAsync(assignment, resolved.Value.ResultType, module, ex, instanceIndex, executionTimer).ConfigureAwait(false);
        }
    }

    private async Task ExecuteAndPublishAsync(
        ModuleAssignment assignment,
        IModule module,
        int instanceIndex,
        DistributedModuleExecutionTimer executionTimer,
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
                var downloadStartedAt = Stopwatch.GetTimestamp();
                try
                {
                    await _artifactLifecycleManager.DownloadConsumedArtifactsAsync(moduleType, cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    executionTimer.ArtifactDownloadDuration = Stopwatch.GetElapsedTime(downloadStartedAt);
                }
            }

            var moduleState = new ModuleState(module, moduleType);
            _executionLocationContext?.RestoreSatisfiedConditionGroups(
                module,
                assignment.SatisfiedConditionGroups);
            ModuleStateDependencyInitializer.Populate(
                moduleState,
                _typeRegistry.GetRegisteredModuleTypes(),
                _dependencyRegistry,
                _metadataRegistry);
            IModuleResult? result;
            executionTimer.StartExecution();
            try
            {
                await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, cancellationToken).ConfigureAwait(false);

                result = await module.AsInternal().ResultTask.ConfigureAwait(false);
            }
            finally
            {
                executionTimer.FinishExecution();
            }

            IReadOnlyList<ArtifactReference>? artifactReferences;
            var uploadStartedAt = Stopwatch.GetTimestamp();
            try
            {
                artifactReferences = await TryUploadArtifactsAsync(
                    module,
                    assignment.ModuleTypeName,
                    moduleLogger,
                    cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                executionTimer.ArtifactUploadDuration = Stopwatch.GetElapsedTime(uploadStartedAt);
            }

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

            serialized = serialized with { ExecutionTelemetry = executionTimer.CreateTelemetry() };
            await _coordinator.PublishResultAsync(serialized, cancellationToken).ConfigureAwait(false);
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
        DistributedModuleExecutionTimer executionTimer)
    {
        try
        {
            var resultTask = module.AsInternal().ResultTask;
            // A transport failure cannot replace an outcome already accepted by the module.
            var terminalResult = resultTask.IsCompletedSuccessfully
                ? resultTask.Result
                : ModuleResultFactory.CreateException(
                    resultType,
                    exception,
                    new ModuleExecutionContext(module, module.GetType())
                    {
                        Status = exception is OperationCanceledException ? ModuleStatus.Cancelled : ModuleStatus.Failed,
                        Exception = exception,
                    });
            SerializedModuleResult serialized;
            try
            {
                serialized = _serializer.Serialize(
                    terminalResult,
                    assignment.ModuleTypeName,
                    assignment.ResultTypeName,
                    instanceIndex);
            }
            catch (Exception serializationException) when (resultTask.IsCompletedSuccessfully)
            {
                // An accepted outcome that cannot cross the wire must still complete the master's waiter.
                var failure = ModuleResultFactory.CreateException(
                    resultType,
                    serializationException,
                    new ModuleExecutionContext(module, module.GetType())
                    {
                        Status = ModuleStatus.Failed,
                        Exception = serializationException,
                    });
                serialized = _serializer.Serialize(
                    failure,
                    assignment.ModuleTypeName,
                    assignment.ResultTypeName,
                    instanceIndex);
            }

            serialized = serialized with { ExecutionTelemetry = executionTimer.CreateTelemetry() };
            await DistributedFailurePublisher.PublishAsync(_coordinator, serialized).ConfigureAwait(false);
        }
        catch (Exception publishException)
        {
            _logger.LogCritical(publishException,
                "Failed to publish failure result for module {Module} — master may hang waiting for this result",
                assignment.ModuleTypeName);
        }
    }
}
