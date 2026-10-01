namespace ModularPipelines.Distributed.Coordination;

/// <summary>
/// Creates a coordinator through <see cref="IDistributedCoordinatorFactory"/> on first use, so building
/// the service provider never blocks on connections or master discovery, and disposes it with the host.
/// </summary>
internal abstract class DeferredCoordinator<TCoordinator>(Func<CancellationToken, Task<TCoordinator>> create) : IAsyncDisposable, IDisposable
    where TCoordinator : class
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly Func<CancellationToken, Task<TCoordinator>> _create = create;
    private volatile TCoordinator? _inner;
    private int _disposeState;

    protected async ValueTask<TCoordinator> GetAsync(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposeState) != 0, this);
        if (_inner is { } inner)
        {
            return inner;
        }

        await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposeState) != 0, this);
            return _inner ??= await _create(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposeState, 1) != 0)
        {
            return;
        }

        await _lock.WaitAsync().ConfigureAwait(false);
        try
        {
            switch (_inner)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
        finally
        {
            _lock.Release();
        }

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

    public async Task<bool> IsMasterLostAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .IsMasterLostAsync(cancellationToken).ConfigureAwait(false);
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

    public async Task<bool> IsMasterLostAsync(CancellationToken cancellationToken) =>
        await (await GetAsync(cancellationToken).ConfigureAwait(false))
            .IsMasterLostAsync(cancellationToken).ConfigureAwait(false);
}
