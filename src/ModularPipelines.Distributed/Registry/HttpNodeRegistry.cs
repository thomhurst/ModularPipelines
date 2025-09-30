using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Options;

namespace ModularPipelines.Distributed.Registry;

/// <summary>
/// HTTP-based in-memory implementation of <see cref="INodeRegistry"/>.
/// Tracks worker nodes with heartbeat monitoring and automatic cleanup of stale workers.
/// </summary>
internal sealed class HttpNodeRegistry : INodeRegistry
{
    private readonly ConcurrentDictionary<string, WorkerNode> _workers = new();
    private readonly ILogger<HttpNodeRegistry> _logger;
    private readonly DistributedPipelineOptions _options;
    private readonly object _lock = new();

    public HttpNodeRegistry(
        ILogger<HttpNodeRegistry> logger,
        IOptions<DistributedPipelineOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public Task RegisterWorkerAsync(WorkerNode workerNode, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workerNode);

        workerNode.LastHeartbeat = DateTimeOffset.UtcNow;
        workerNode.Status = WorkerStatus.Available;

        var added = _workers.TryAdd(workerNode.Id, workerNode);

        if (added)
        {
            _logger.LogInformation(
                "Worker {WorkerId} registered with endpoint {Endpoint}. OS: {Os}, Max Parallel: {MaxParallel}",
                workerNode.Id,
                workerNode.Endpoint,
                workerNode.Capabilities.Os,
                workerNode.Capabilities.MaxParallelModules);
        }
        else
        {
            _logger.LogWarning(
                "Worker {WorkerId} was already registered. Updating registration.",
                workerNode.Id);
            _workers[workerNode.Id] = workerNode;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UnregisterWorkerAsync(string workerId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workerId);

        if (_workers.TryRemove(workerId, out var worker))
        {
            _logger.LogInformation(
                "Worker {WorkerId} unregistered from endpoint {Endpoint}",
                workerId,
                worker.Endpoint);
        }
        else
        {
            _logger.LogWarning("Attempted to unregister unknown worker {WorkerId}", workerId);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IReadOnlyCollection<WorkerNode>> GetAvailableWorkersAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var timeout = _options.WorkerHeartbeatTimeout;

        var availableWorkers = _workers.Values
            .Where(w => w.Status != WorkerStatus.Offline &&
                       w.Status != WorkerStatus.Draining &&
                       (now - w.LastHeartbeat) < timeout)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<WorkerNode>>(availableWorkers);
    }

    /// <inheritdoc />
    public Task UpdateHeartbeatAsync(string workerId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workerId);

        if (_workers.TryGetValue(workerId, out var worker))
        {
            worker.LastHeartbeat = DateTimeOffset.UtcNow;

            // If worker was previously offline, mark as available
            if (worker.Status == WorkerStatus.Offline)
            {
                worker.Status = WorkerStatus.Available;
                _logger.LogInformation("Worker {WorkerId} is back online", workerId);
            }

            _logger.LogDebug(
                "Heartbeat received from worker {WorkerId}. Current load: {CurrentLoad}/{MaxLoad}",
                workerId,
                worker.CurrentLoad,
                worker.Capabilities.MaxParallelModules);
        }
        else
        {
            _logger.LogWarning(
                "Received heartbeat from unregistered worker {WorkerId}",
                workerId);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveStaleWorkersAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var timeout = _options.WorkerHeartbeatTimeout;

        var staleWorkers = _workers.Values
            .Where(w => (now - w.LastHeartbeat) > timeout)
            .ToList();

        foreach (var worker in staleWorkers)
        {
            if (worker.Status != WorkerStatus.Offline)
            {
                worker.Status = WorkerStatus.Offline;

                _logger.LogWarning(
                    "Worker {WorkerId} marked as offline. Last heartbeat: {LastHeartbeat} (timeout: {Timeout})",
                    worker.Id,
                    worker.LastHeartbeat,
                    timeout);
            }

            // Optionally remove offline workers after a longer period
            if ((now - worker.LastHeartbeat) > timeout.Add(timeout))
            {
                _workers.TryRemove(worker.Id, out _);
                _logger.LogInformation(
                    "Worker {WorkerId} removed from registry after extended offline period",
                    worker.Id);
            }
        }

        return Task.CompletedTask;
    }
}
