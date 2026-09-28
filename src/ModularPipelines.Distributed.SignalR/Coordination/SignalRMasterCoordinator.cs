using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Distributed.SignalR.Server;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Master-side <see cref="IDistributedMasterCoordinator"/> backed by SignalR. Work, leases, results
/// and cancellation live in the in-memory coordinator; workers reach it through the hub by pulling
/// leases, so ordering, lease expiry and per-worker parallelism match every other backend.
/// </summary>
internal sealed class SignalRMasterCoordinator(
    InMemoryDistributedCoordinator coordinator,
    MasterServerHost? serverHost = null) : IDistributedMasterCoordinator, IAsyncDisposable
{
    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken) =>
        coordinator.EnqueueModuleAsync(assignment, cancellationToken);

    public Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        coordinator.WithdrawAssignmentAsync(moduleId, cancellationToken);

    public Task<IReadOnlyList<ModuleLease>> GetActiveLeasesAsync(CancellationToken cancellationToken) =>
        coordinator.GetActiveLeasesAsync(cancellationToken);

    public Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken) =>
        coordinator.RequeueExpiredLeasesAsync(cancellationToken);

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken) =>
        coordinator.GetRegisteredWorkersAsync(cancellationToken);

    public Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(CancellationToken cancellationToken) =>
        coordinator.GetWorkerStatusesAsync(cancellationToken);

    public Task SignalCompletionAsync(CancellationToken cancellationToken) =>
        coordinator.SignalCompletionAsync(cancellationToken);

    public Task BroadcastCancellationAsync(DistributedCancellationReason reason, CancellationToken cancellationToken) =>
        coordinator.BroadcastCancellationAsync(reason, cancellationToken);

    public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken) =>
        coordinator.RegisterWorkerAsync(registration, cancellationToken);

    public Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken) =>
        coordinator.DequeueModuleAsync(workerId, workerCapabilities, cancellationToken);

    public Task PublishResultAsync(SerializedModuleResult result, ModuleLease? lease, CancellationToken cancellationToken) =>
        coordinator.PublishResultAsync(result, lease, cancellationToken);

    public Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        coordinator.WaitForResultAsync(moduleId, cancellationToken);

    public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
        coordinator.SendHeartbeatAsync(status, cancellationToken);

    public Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken) =>
        coordinator.WaitForCancellationAsync(cancellationToken);

    public async ValueTask DisposeAsync()
    {
        if (serverHost is not null)
        {
            await serverHost.DisposeAsync().ConfigureAwait(false);
        }
    }
}
