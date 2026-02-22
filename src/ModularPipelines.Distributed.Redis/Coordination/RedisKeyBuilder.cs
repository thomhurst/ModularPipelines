namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Generates all Redis keys with pattern {prefix}:{runId}:{purpose}.
/// </summary>
internal class RedisKeyBuilder
{
    private readonly string _prefix;
    private readonly string _runId;

    public RedisKeyBuilder(string prefix, string runId)
    {
        _prefix = prefix;
        _runId = runId;
    }

    public string WorkQueue => $"{_prefix}:{_runId}:work:queue";

    public string Results => $"{_prefix}:{_runId}:results";

    public string ResultChannel(string moduleTypeName) => $"{_prefix}:{_runId}:results:{moduleTypeName}";

    public string Workers => $"{_prefix}:{_runId}:workers";

    public string Heartbeats => $"{_prefix}:{_runId}:heartbeats";

    public string Cancellation => $"{_prefix}:{_runId}:cancellation";

    public string CancellationChannel => $"{_prefix}:{_runId}:cancellation:signal";

    // Artifact keys
    public string ArtifactMeta(string artifactId) => $"{_prefix}:{_runId}:artifacts:meta:{artifactId}";

    public string ArtifactData(string artifactId) => $"{_prefix}:{_runId}:artifacts:data:{artifactId}";

    public string ArtifactChunk(string artifactId, int chunkIndex) => $"{_prefix}:{_runId}:artifacts:data:{artifactId}:chunk:{chunkIndex}";

    public string ArtifactIndex(string moduleTypeName) => $"{_prefix}:{_runId}:artifacts:index:{moduleTypeName}";

    /// <summary>
    /// Returns all non-channel keys (for setting expiration).
    /// </summary>
    public IEnumerable<string> AllStorageKeys =>
    [
        WorkQueue,
        Results,
        Workers,
        Heartbeats,
        Cancellation,
    ];
}
