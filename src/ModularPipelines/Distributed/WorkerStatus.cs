namespace ModularPipelines.Distributed;

/// <summary>
/// Reports a distributed worker's liveness, its in-flight work and, finally, its command metrics.
/// </summary>
public sealed record WorkerStatus
{
    /// <summary>Gets the reporting worker.</summary>
    public required WorkerId WorkerId { get; init; }

    /// <summary>Gets the pipeline execution this status belongs to.</summary>
    public string? RunId { get; init; }

    /// <summary>
    /// Gets the modules the worker is executing. Each heartbeat renews the leases on these modules.
    /// </summary>
    public IReadOnlyList<ModuleId> InFlightModules { get; init; } = [];

    /// <summary>
    /// Gets whether this is the worker's final status, published after it stopped executing work.
    /// A final status carries the worker's command metrics and keeps its registration visible after
    /// its heartbeat expires.
    /// </summary>
    public bool IsFinal { get; init; }

    /// <summary>Gets the worker's count of commands executed outside a module context.</summary>
    public int UnattributedCommandCount { get; init; }

    /// <summary>Gets the worker's command counts by module identifier.</summary>
    public IReadOnlyDictionary<ModuleId, int> ModuleCommandCounts { get; init; } =
        new Dictionary<ModuleId, int>();

    /// <summary>
    /// Determines whether a worker is live. A final status keeps a worker visible after its
    /// heartbeat expires.
    /// </summary>
    internal static bool IsLive(WorkerStatus? status, bool hasLiveHeartbeat) =>
        status?.IsFinal == true || hasLiveHeartbeat;
}
