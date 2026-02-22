namespace ModularPipelines.Distributed;

/// <summary>
/// Defines the coordination layer for distributed pipeline execution.
/// Implement this interface to provide a custom coordination transport (Redis, HTTP, shared filesystem, etc.).
/// </summary>
public interface IDistributedCoordinator
{
    // Work Queue
    Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken);
    Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken);

    // Results
    Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken);
    Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken);

    // Worker Management
    Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken);
    Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken);
    Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken);
    Task<WorkerHeartbeat?> GetLastHeartbeatAsync(int workerIndex, CancellationToken cancellationToken);
    Task UpdateWorkerStatusAsync(int workerIndex, WorkerStatus status, CancellationToken cancellationToken);

    // Cancellation
    Task BroadcastCancellationAsync(string reason, CancellationToken cancellationToken);
    Task<CancellationSignal?> IsCancellationRequestedAsync(CancellationToken cancellationToken);
}
