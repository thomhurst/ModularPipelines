namespace ModularPipelines.Distributed;

/// <summary>
/// Defines the master-side coordination operations used during distributed pipeline execution.
/// Masters also implement worker operations because the master process participates in local execution.
/// </summary>
public interface IDistributedMasterCoordinator : IDistributedWorkerCoordinator
{
    /// <summary>
    /// Adds a module assignment to the distributed work queue.
    /// </summary>
    Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken);

    /// <summary>
    /// Waits for the result of a distributed module assignment.
    /// </summary>
    Task<SerializedModuleResult> WaitForResultAsync(
        string moduleTypeName,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets workers whose registrations are still live.
    /// </summary>
    Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(
        CancellationToken cancellationToken);

    /// <summary>
    /// Signals that no more module assignments will be produced.
    /// </summary>
    Task SignalCompletionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Broadcasts cancellation to distributed workers.
    /// </summary>
    Task BroadcastCancellationAsync(CancellationToken cancellationToken);
}
