using System.Collections.Concurrent;
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
using ModularPipelines.Models;
using ModularPipelines.Modules;

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
    DistributedConditionRouting? conditionRouting = null) : IExecutionBackend
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IDistributedWorkerCoordinator _coordinator = coordinator;
    private readonly IReadOnlyList<IModule> _registeredModules = registeredModules
        .Distinct<IModule>(ReferenceEqualityComparer.Instance)
        .ToArray();

    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly IParallelLimitProvider _parallelLimitProvider = parallelLimitProvider;
    private readonly ILogger<WorkerModuleExecutor> _logger = logger;
    private readonly DistributedConditionRouting? _conditionRouting = conditionRouting;
    private readonly DistributedAssignmentExecutor _assignmentExecutor = new(
        typeRegistry,
        serializer,
        moduleRunner,
        resultRegistry,
        dependencyRegistry,
        metadataRegistry,
        serviceScopeFactory,
        artifactLifecycleManager,
        coordinator,
        logger);

    public bool OwnsEntirePlan => false;

    public async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IReadOnlyList<IModule> modules,
        IExecutionBackendContext context,
        CancellationToken cancellationToken)
    {
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
        var capabilities = BuildCapabilities(options);
        var maxConcurrency = DistributedWorkerPool.GetMaxConcurrency(
            _parallelLimitProvider,
            options);
        await RegisterWorkerAsync(options.InstanceIndex, capabilities, cancellationToken);
        var heartbeatTask = SendHeartbeatsAsync(
            options.InstanceIndex,
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
                async (assignment, token) =>
                {
                    _logger.LogInformation("Worker {Index} executing module {Module}",
                        options.InstanceIndex, assignment.ModuleTypeName);
                    var executedModule = await _assignmentExecutor.ExecuteAsync(
                            assignment,
                            moduleLookup,
                            options.InstanceIndex,
                            (module, currentAssignment) =>
                                _conditionRouting?.RestoreLocallySatisfiedGroups(
                                    module,
                                    currentAssignment.SatisfiedConditionGroups),
                            token)
                        .ConfigureAwait(false);
                    if (executedModule is not null)
                    {
                        executedModules.Enqueue(executedModule);
                    }
                },
                maxConcurrency,
                exception => _logger.LogError(
                    exception,
                    "Worker {Index} encountered an error in execution loop",
                    options.InstanceIndex),
                cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Worker {Index} stopped dequeuing assignments because cancellation was requested",
                    options.InstanceIndex);
            }
        }
        finally
        {
            await executionCts.CancelAsync();
            await AwaitBackgroundTasksAsync(heartbeatTask, cancellationTask);
        }

        return executedModules
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
            RunIdentifier = _options.Value.RunIdentifier,
        };
        await _coordinator.RegisterWorkerAsync(registration, cancellationToken);
        _logger.LogInformation("Worker {Index} registered with capabilities: {Capabilities}",
            instanceIndex, string.Join(", ", capabilities));
    }
}
