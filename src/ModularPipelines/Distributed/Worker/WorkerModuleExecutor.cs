using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts;
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
    IExecutionLocationContext? executionLocationContext = null,
    LocalCapabilityRegistry? localCapabilities = null,
    AcceptedArtifactRegistry? acceptedArtifacts = null) : IExecutionBackend
{
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly IDistributedWorkerCoordinator _coordinator = coordinator;
    private readonly IReadOnlyList<IModule> _registeredModules = [.. registeredModules.Distinct<IModule>(ReferenceEqualityComparer.Instance)];
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly IModuleResultRegistry _resultRegistry = resultRegistry;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly IParallelLimitProvider _parallelLimitProvider = parallelLimitProvider;
    private readonly ILogger<WorkerModuleExecutor> _logger = logger;
    private readonly DistributedAssignmentExecutor _assignmentExecutor = new(
        coordinator,
        typeRegistry,
        serializer,
        moduleRunner,
        resultRegistry,
        dependencyRegistry,
        metadataRegistry,
        serviceScopeFactory,
        artifactLifecycleManager,
        acceptedArtifacts,
        executionLocationContext,
        logger);

    public bool OwnsEntirePlan => false;

    // Workers execute whatever the master assigns, so the plan's duration estimates only
    // influence the master's scheduling and are not consulted here.
    public async Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        ExecutionBackendRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var options = _options.Value;
        var workerId = options.LocalWorkerId;

        // The worker token stops everything (host shutdown, a Stopped broadcast or completion).
        // The pipeline token additionally stops non-AlwaysRun work after a PipelineFailed broadcast.
        using var workerCts = CancellationTokenSource.CreateLinkedTokenSource(
            _lifetime.ApplicationStopping,
            cancellationToken);
        using var pipelineCts = CancellationTokenSource.CreateLinkedTokenSource(workerCts.Token);
        var workerToken = workerCts.Token;
        var availableModules = _registeredModules
            .Concat(request.Modules)
            .Distinct<IModule>(ReferenceEqualityComparer.Instance)
            .ToArray();

        foreach (var module in availableModules)
        {
            _typeRegistry.Register(module.GetType());
        }

        var moduleLookup = DependencyResultApplicator.BuildModuleLookup(availableModules);
        var dependencyResultCache = new DependencyResultCache(_coordinator, workerToken);
        var capabilities = await LocalCapabilities.GetAsync(localCapabilities, options, workerToken)
            .ConfigureAwait(false);
        var maxConcurrency = DistributedWorkerPool.GetMaxConcurrency(
            _parallelLimitProvider,
            options);
        await RegisterWorkerAsync(workerId, capabilities, maxConcurrency, workerToken).ConfigureAwait(false);

        var inFlightLeases = new InFlightLeases();
        var heartbeatTask = SendHeartbeatsAsync(
            workerId,
            options.RunId,
            options.WorkerHeartbeatInterval,
            inFlightLeases,
            workerToken);
        var cancellationTask = ObserveDistributedCancellationAsync(
            workerCts,
            pipelineCts,
            options.WorkerHeartbeatInterval);

        var executedModules = new ConcurrentQueue<IModule>();
        try
        {
            _logger.LogDebug(
                "Worker {WorkerId} starting {MaxConcurrency} concurrent execution slot(s)",
                workerId,
                maxConcurrency);
            await DistributedWorkerPool.RunAsync(
                token => _coordinator.DequeueModuleAsync(workerId, capabilities, token),
                async (lease, claimedAt, _) =>
                {
                    _logger.LogDebug(
                        "Worker {WorkerId} executing module {Module}",
                        workerId,
                        lease.Assignment.ModuleId);
                    var executionToken = lease.Assignment.AlwaysRun ? workerToken : pipelineCts.Token;
                    var module = await _assignmentExecutor.ExecuteAsync(
                            lease,
                            claimedAt,
                            moduleLookup,
                            dependencyResultCache,
                            executionToken)
                        .ConfigureAwait(false);

                    // A claimed module belongs in this worker's summary whether it succeeds or fails.
                    if (module is not null)
                    {
                        executedModules.Enqueue(module);
                    }
                },
                maxConcurrency,
                exception => _logger.LogError(
                    exception,
                    "Worker {WorkerId} encountered an error in execution loop",
                    workerId),
                workerToken,
                inFlightLeases: inFlightLeases).ConfigureAwait(false);
        }
        finally
        {
            await workerCts.CancelAsync().ConfigureAwait(false);
            await AwaitBackgroundTasksAsync(heartbeatTask, cancellationTask).ConfigureAwait(false);
        }

        return _resultRegistry.GetCompletedResults(executedModules);
    }

    internal Task<IReadOnlyList<IModuleResult>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        return ExecuteAsync(
            new ExecutionBackendRequest
            {
                Modules = modules,
                EstimatedDurations = new Dictionary<ModuleId, TimeSpan>(),
                Context = new ExecutionBackendContext(_resultRegistry),
            },
            CancellationToken.None);
    }

    private async Task SendHeartbeatsAsync(
        WorkerId workerId,
        string? runIdentifier,
        TimeSpan interval,
        InFlightLeases inFlightLeases,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(interval, cancellationToken).ConfigureAwait(false);
                await _coordinator.SendHeartbeatAsync(
                        new WorkerStatus
                        {
                            WorkerId = workerId,
                            RunId = runIdentifier,
                            InFlightModules = inFlightLeases.GetModuleIds(),
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
                _logger.LogWarning(ex, "Worker {WorkerId} heartbeat failed", workerId);
            }
        }
    }

    private async Task ObserveDistributedCancellationAsync(
        CancellationTokenSource workerCts,
        CancellationTokenSource pipelineCts,
        TimeSpan retryInterval)
    {
        while (!workerCts.IsCancellationRequested)
        {
            try
            {
                var reason = await _coordinator.WaitForCancellationAsync(workerCts.Token).ConfigureAwait(false);
                if (reason == DistributedCancellationReason.PipelineFailed)
                {
                    // Keep claiming AlwaysRun work; the coordinator only returns AlwaysRun leases now.
                    _logger.LogInformation(
                        "Coordinator reported a pipeline failure; cancelling non-AlwaysRun modules");
                    await pipelineCts.CancelAsync().ConfigureAwait(false);
                }
                else
                {
                    _logger.LogInformation("Coordinator requested distributed cancellation");
                    await workerCts.CancelAsync().ConfigureAwait(false);
                }

                return;
            }
            catch (OperationCanceledException) when (workerCts.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Worker cancellation observer failed; retrying");
            }

            try
            {
                await Task.Delay(retryInterval, workerCts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (workerCts.IsCancellationRequested)
            {
                return;
            }
        }
    }

    private static async Task AwaitBackgroundTasksAsync(params Task[] tasks)
    {
        try
        {
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task RegisterWorkerAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> capabilities,
        int maxConcurrency,
        CancellationToken cancellationToken)
    {
        var registration = new WorkerRegistration
        {
            WorkerId = workerId,
            Capabilities = [.. capabilities],
            RegisteredAt = DateTimeOffset.UtcNow,
            MaxParallelism = maxConcurrency,
            RunId = _options.Value.RunId,
            PipelineSchemaVersion = _typeRegistry.GetPipelineSchemaVersion(),
        };
        await _coordinator.RegisterWorkerAsync(registration, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation(
            "Worker {WorkerId} registered with capabilities: {Capabilities}",
            workerId,
            string.Join(", ", capabilities));
    }
}
