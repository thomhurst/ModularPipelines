using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Generates Redis keys with pattern {prefix}:{{runId}}:{purpose}. The braces form a
/// Redis Cluster hash tag so every key for one run shares a slot for multi-key scripts.
/// Run identifiers are restricted to <c>[A-Za-z0-9._-]</c> by the options pipeline, so they
/// cannot contain the braces that would change the hash tag.
/// </summary>
internal class RedisKeyBuilder(string prefix, string runId)
{
    private readonly string _runPrefix = $"{prefix}:{{{runId}}}";

    public string WorkQueue => $"{_runPrefix}:work:queue";

    /// <summary>Gets the hash of active leases, keyed by module identifier.</summary>
    public string Leases => $"{_runPrefix}:work:leases";

    public string Results => $"{_runPrefix}:results";

    public string ResultChannel(ModuleId moduleId) => $"{_runPrefix}:results:{moduleId}";

    public string Workers => $"{_runPrefix}:workers";

    public string WorkerStatuses => $"{_runPrefix}:workers:status";

    public string WorkerHeartbeatField(WorkerId workerId) => $"heartbeat:{workerId}";

    public string WorkAvailableChannel => $"{_runPrefix}:work:available";

    public string CompletionFlag => $"{_runPrefix}:completion";

    public string CompletionChannel => $"{_runPrefix}:completion:signal";

    public string CancellationFlag => $"{_runPrefix}:cancellation";

    public string CancellationChannel => $"{_runPrefix}:cancellation:signal";

    /// <summary>
    /// Gets the key holding the Redis server time of the master's latest heartbeat. Only the master's
    /// heartbeats renew it, so it expires once the master stops.
    /// </summary>
    public string MasterHeartbeat => $"{_runPrefix}:master:heartbeat";

    /// <summary>
    /// Gets the key marking that the master started. It is renewed with the other run keys, so it
    /// outlives an expired heartbeat and a stopped master is reported as lost rather than not started.
    /// </summary>
    public string MasterStarted => $"{_runPrefix}:master:started";

    // Artifact keys
    public string ArtifactMeta(string artifactId) => $"{_runPrefix}:artifacts:meta:{artifactId}";

    public string ArtifactData(string artifactId) => $"{_runPrefix}:artifacts:data:{artifactId}";

    public string ArtifactChunk(string artifactId, int chunkIndex) => $"{_runPrefix}:artifacts:data:{artifactId}:chunk:{chunkIndex}";

    public string ArtifactIndex(ModuleId moduleId) => $"{_runPrefix}:artifacts:index:{moduleId}";

    /// <summary>
    /// Returns the coordination keys whose expiry is refreshed while the run is active.
    /// </summary>
    public IReadOnlyList<string> CoordinationKeys =>
    [
        WorkQueue,
        Leases,
        Results,
        Workers,
        WorkerStatuses,
        CompletionFlag,
        CancellationFlag,
        MasterStarted,
    ];
}
