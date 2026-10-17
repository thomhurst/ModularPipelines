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

    /// <summary>
    /// Gets the sorted set of queued assignments. Every member has score zero and sorts by
    /// <c>{class}|{order}|{assignment}</c>, so each capability class is a contiguous lexicographic range.
    /// </summary>
    public string WorkQueue => $"{_runPrefix}:work:queue";

    /// <summary>Gets the hash of capability classes with queued work, keyed by class identifier.</summary>
    public string QueueClasses => $"{_runPrefix}:work:classes";

    /// <summary>Gets the hash of queue members, keyed by module identifier, so withdrawal needs no queue scan.</summary>
    public string QueueIndex => $"{_runPrefix}:work:index";

    /// <summary>Gets the hash of active leases, keyed by module identifier.</summary>
    public string Leases => $"{_runPrefix}:work:leases";

    /// <summary>Gets the module for each claim token, retained until run cleanup to fence late retries.</summary>
    public string Claims => $"{_runPrefix}:work:claims";

    /// <summary>Gets lost workers for each module's expired leases, retained across claims.</summary>
    public string LeaseExpirationWorkers => $"{_runPrefix}:work:lease-expirations";

    public string Results => $"{_runPrefix}:results";

    public string ResultChannel(ModuleId moduleId) => $"{_runPrefix}:results:{moduleId}";

    public string Workers => $"{_runPrefix}:workers";

    /// <summary>
    /// Gets the hash of worker heartbeats. Each field expires after the worker timeout, so the hash
    /// holds exactly the live workers.
    /// </summary>
    public string WorkerHeartbeats => $"{_runPrefix}:workers:heartbeat";

    public string WorkerStatuses => $"{_runPrefix}:workers:status";

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

    /// <summary>Gets the set of artifact keys renewed by the master's heartbeat, including partial uploads.</summary>
    public string ArtifactKeys => $"{_runPrefix}:artifacts:keys";

    /// <summary>Gets the set of modules that uploaded artifacts in this run.</summary>
    public string ArtifactModules => $"{_runPrefix}:artifacts:modules";

    /// <summary>
    /// Returns the coordination keys whose expiry is refreshed while the run is active, in the order
    /// <see cref="RedisCoordinationScripts"/> expects them as <c>KEYS[1]</c> to <c>KEYS[12]</c>.
    /// </summary>
    public IReadOnlyList<string> CoordinationKeys =>
    [
        WorkQueue,
        QueueClasses,
        QueueIndex,
        Leases,
        Results,
        Workers,
        WorkerHeartbeats,
        WorkerStatuses,
        CompletionFlag,
        CancellationFlag,
        MasterStarted,
        LeaseExpirationWorkers,
    ];

    /// <summary>
    /// Returns every key a coordination script may touch: <see cref="CoordinationKeys"/> followed by
    /// <see cref="MasterHeartbeat"/> as <c>KEYS[13]</c> and <see cref="Claims"/> as <c>KEYS[14]</c>.
    /// Declaring every key keeps scripts Redis Cluster safe.
    /// </summary>
    public IReadOnlyList<string> ScriptKeys => [.. CoordinationKeys, MasterHeartbeat, Claims];

    /// <summary>
    /// Gets the coordination keys deleted when the run ends. The completion flag is kept so workers that
    /// start late see a completed master.
    /// </summary>
    public IReadOnlyList<string> RunCleanupKeys =>
    [
        .. CoordinationKeys.Where(key => key != CompletionFlag),
        MasterHeartbeat,
        Claims,
    ];
}
