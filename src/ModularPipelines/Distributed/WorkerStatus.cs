namespace ModularPipelines.Distributed;

/// <summary>
/// Reports a distributed worker's liveness and execution telemetry.
/// </summary>
public record WorkerStatus(
    int WorkerIndex)
{
    /// <summary>
    /// Gets the pipeline execution this status belongs to, when available.
    /// </summary>
    public string? RunIdentifier { get; init; }

    /// <summary>
    /// Gets the worker's final count of commands executed outside a module context, when available.
    /// </summary>
    public int? UnattributedCommandCount { get; init; }

    /// <summary>
    /// Gets the worker's final command counts by stable module type identifier, when available.
    /// </summary>
    public IReadOnlyDictionary<string, int>? ModuleCommandCounts { get; init; }

    /// <summary>
    /// Determines whether a status or heartbeat represents a live worker.
    /// Final metrics keep a worker visible after its heartbeat expires.
    /// </summary>
    /// <param name="status">The latest worker status, when one has been received.</param>
    /// <param name="hasLiveHeartbeat">Whether the worker has a non-expired heartbeat.</param>
    /// <returns><see langword="true"/> when the worker is live or has published final metrics.</returns>
    public static bool IsLive(WorkerStatus? status, bool hasLiveHeartbeat) =>
        status?.UnattributedCommandCount.HasValue == true || hasLiveHeartbeat;
}
