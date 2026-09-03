namespace ModularPipelines.Distributed;

/// <summary>
/// Factory for creating distributed coordinators with async initialization.
/// Use when the coordination provider requires async setup (connecting to a server, creating queues, etc.).
/// </summary>
public interface IDistributedCoordinatorFactory
{
    /// <summary>
    /// Creates the coordinator used by the master process.
    /// </summary>
    Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Creates the coordinator used by a worker process.
    /// </summary>
    Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken);
}
