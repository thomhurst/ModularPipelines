namespace ModularPipelines.Distributed;

/// <summary>
/// Defines the worker-side coordination operations used during distributed pipeline execution.
/// </summary>
public interface IDistributedWorkerCoordinator
{
    /// <summary>
    /// Waits for the next module assignment that the worker can execute.
    /// </summary>
    Task<ModuleAssignment?> DequeueModuleAsync(
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken);

    /// <summary>
    /// Publishes a completed module result to the master.
    /// </summary>
    Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken);

    /// <summary>
    /// Registers a worker and its capabilities with the master.
    /// </summary>
    Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken);

    /// <summary>
    /// Reports that a registered worker is still alive.
    /// </summary>
    Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken);

    /// <summary>
    /// Waits until the master broadcasts distributed cancellation.
    /// </summary>
    Task WaitForCancellationAsync(CancellationToken cancellationToken);
}
