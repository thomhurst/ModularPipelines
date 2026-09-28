namespace ModularPipelines.Distributed;

/// <summary>
/// Defines the master-side coordination operations used during distributed pipeline execution.
/// Masters also implement worker operations because the master process participates in local execution.
/// </summary>
/// <remarks>
/// Follows the evolution policy described on <see cref="IDistributedWorkerCoordinator"/>.
/// </remarks>
public interface IDistributedMasterCoordinator : IDistributedWorkerCoordinator
{
    /// <summary>
    /// Adds a module assignment to the distributed work queue.
    /// </summary>
    Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a queued assignment that no worker has claimed.
    /// </summary>
    /// <returns><see langword="true"/> when the assignment was queued and has been removed.</returns>
    Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the leases currently held by workers.
    /// </summary>
    Task<IReadOnlyList<ModuleLease>> GetActiveLeasesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Returns assignments whose leases expired without a published result to the queue.
    /// A lease expires when its worker has not renewed it for <see cref="DistributedOptions.WorkerTimeout"/>.
    /// </summary>
    /// <returns>The modules that were queued again.</returns>
    Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets workers whose registrations are live or that have published a final status.
    /// </summary>
    Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the latest status reported by each worker.
    /// </summary>
    /// <returns>At most one status for each worker identifier.</returns>
    Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(
        CancellationToken cancellationToken);

    /// <summary>
    /// Signals that no more module assignments will be produced. Pending and later dequeues return
    /// <see langword="null"/>.
    /// </summary>
    Task SignalCompletionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Broadcasts cancellation to distributed workers. The first broadcast reason is durable:
    /// workers that start waiting later observe it immediately.
    /// </summary>
    Task BroadcastCancellationAsync(
        DistributedCancellationReason reason,
        CancellationToken cancellationToken);
}
