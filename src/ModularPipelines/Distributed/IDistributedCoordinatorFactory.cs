namespace ModularPipelines.Distributed;

/// <summary>
/// Factory for creating distributed coordinators with async initialization.
/// Use when the coordination provider requires async setup (connecting to a server, creating queues, etc.).
/// </summary>
public interface IDistributedCoordinatorFactory
{
    Task<IDistributedCoordinator> CreateAsync(CancellationToken cancellationToken);
}
