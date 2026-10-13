namespace ModularPipelines.Distributed.Coordination;

/// <summary>
/// Creates a coordinator through <see cref="IDistributedCoordinatorFactory"/> on first use, so building
/// the service provider never blocks on connections or master discovery, and disposes it with the host.
/// </summary>
internal abstract class DeferredCoordinator<TCoordinator>(Func<CancellationToken, Task<TCoordinator>> create)
    : IDeferredDistributedBackend, IAsyncDisposable, IDisposable
    where TCoordinator : class
{
    private readonly DeferredBackend<TCoordinator> _coordinator = new(create);

    protected ValueTask<TCoordinator> GetAsync(CancellationToken cancellationToken) =>
        _coordinator.GetAsync(cancellationToken);

    public async ValueTask<object> GetBackendAsync(CancellationToken cancellationToken) =>
        await GetAsync(cancellationToken).ConfigureAwait(false);

    public async ValueTask DisposeAsync()
    {
        await _coordinator.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();
}

/// <summary>
/// Defers <see cref="IDistributedCoordinatorFactory.CreateMasterAsync"/> to first use.
/// </summary>
internal sealed class DeferredMasterCoordinator(IDistributedCoordinatorFactory factory)
    : DeferredCoordinator<IDistributedMasterCoordinator>(factory.CreateMasterAsync), IDistributedMasterCoordinator
{
    public async Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .EnqueueModuleAsync(assignment, cancellationToken).ConfigureAwait(false);

    public async Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .WithdrawAssignmentAsync(moduleId, cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyList<ModuleLease>> GetActiveLeasesAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .GetActiveLeasesAsync(cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .RequeueExpiredLeasesAsync(cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .GetRegisteredWorkersAsync(cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .GetWorkerStatusesAsync(cancellationToken).ConfigureAwait(false);

    public async Task SignalCompletionAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .SignalCompletionAsync(cancellationToken).ConfigureAwait(false);

    public async Task BroadcastCancellationAsync(
        DistributedCancellationReason reason,
        CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .BroadcastCancellationAsync(reason, cancellationToken).ConfigureAwait(false);

    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .RegisterWorkerAsync(registration, cancellationToken).ConfigureAwait(false);

    public async Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .DequeueModuleAsync(workerId, workerCapabilities, cancellationToken).ConfigureAwait(false);

    public async Task PublishResultAsync(
        SerializedModuleResult result,
        ModuleLease? lease,
        CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .PublishResultAsync(result, lease, cancellationToken).ConfigureAwait(false);

    public async Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .WaitForResultAsync(moduleId, cancellationToken).ConfigureAwait(false);

    public async Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .SendHeartbeatAsync(status, cancellationToken).ConfigureAwait(false);

    public async Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .WaitForCancellationAsync(cancellationToken).ConfigureAwait(false);

    public async Task<DistributedMasterState> GetMasterStateAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .GetMasterStateAsync(cancellationToken).ConfigureAwait(false);
}

/// <summary>
/// Defers <see cref="IDistributedCoordinatorFactory.CreateWorkerAsync"/> to first use so workers do
/// not block while the service provider is built, for example while waiting for master discovery.
/// </summary>
internal sealed class DeferredWorkerCoordinator(IDistributedCoordinatorFactory factory)
    : DeferredCoordinator<IDistributedWorkerCoordinator>(factory.CreateWorkerAsync), IDistributedWorkerCoordinator
{
    public async Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .RegisterWorkerAsync(registration, cancellationToken).ConfigureAwait(false);

    public async Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .DequeueModuleAsync(workerId, workerCapabilities, cancellationToken).ConfigureAwait(false);

    public async Task PublishResultAsync(
        SerializedModuleResult result,
        ModuleLease? lease,
        CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .PublishResultAsync(result, lease, cancellationToken).ConfigureAwait(false);

    public async Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .WaitForResultAsync(moduleId, cancellationToken).ConfigureAwait(false);

    public async Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .SendHeartbeatAsync(status, cancellationToken).ConfigureAwait(false);

    public async Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .WaitForCancellationAsync(cancellationToken).ConfigureAwait(false);

    public async Task<DistributedMasterState> GetMasterStateAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .GetMasterStateAsync(cancellationToken).ConfigureAwait(false);
}
