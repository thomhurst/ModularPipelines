namespace ModularPipelines.Distributed.Abstractions;

/// <summary>
/// Interface for node discovery and registration in a distributed pipeline.
/// </summary>
public interface INodeRegistry
{
    /// <summary>
    /// Registers a worker node with the registry.
    /// </summary>
    /// <param name="workerNode">The worker node information to register.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RegisterWorkerAsync(WorkerNode workerNode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unregisters a worker node from the registry.
    /// </summary>
    /// <param name="workerId">The unique identifier of the worker to unregister.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UnregisterWorkerAsync(string workerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all registered and healthy worker nodes.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of available worker nodes.</returns>
    Task<IReadOnlyCollection<WorkerNode>> GetAvailableWorkersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the heartbeat for a worker node.
    /// </summary>
    /// <param name="workerId">The unique identifier of the worker.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateHeartbeatAsync(string workerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes workers that haven't sent a heartbeat within the timeout period.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveStaleWorkersAsync(CancellationToken cancellationToken = default);
}
