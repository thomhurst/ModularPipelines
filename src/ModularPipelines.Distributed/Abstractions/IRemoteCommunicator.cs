using ModularPipelines.Distributed.Communication.Messages;

namespace ModularPipelines.Distributed.Abstractions;

/// <summary>
/// Interface for remote communication between orchestrator and workers.
/// </summary>
public interface IRemoteCommunicator
{
    /// <summary>
    /// Sends a module execution request to a worker.
    /// </summary>
    /// <param name="worker">The target worker node.</param>
    /// <param name="request">The execution request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The module execution result.</returns>
    Task<ModuleResultResponse> ExecuteModuleAsync(
        WorkerNode worker,
        ModuleExecutionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a health check request to a worker.
    /// </summary>
    /// <param name="worker">The target worker node.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the worker is healthy; otherwise, false.</returns>
    Task<bool> HealthCheckAsync(
        WorkerNode worker,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a cancellation signal to a worker for a specific module execution.
    /// </summary>
    /// <param name="worker">The target worker node.</param>
    /// <param name="executionId">The execution ID to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelExecutionAsync(
        WorkerNode worker,
        string executionId,
        CancellationToken cancellationToken = default);
}
