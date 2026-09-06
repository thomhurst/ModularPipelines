namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// Generates Redis keys with pattern {prefix}:{{runId}}:{purpose}. The braces form a
/// Redis Cluster hash tag so every key for one run shares a slot for multi-key scripts.
/// </summary>
internal class RedisKeyBuilder
{
    private readonly string _runPrefix;

    public RedisKeyBuilder(string prefix, string runId)
    {
        _runPrefix = $"{prefix}:{{{runId}}}";
    }

    public string WorkQueue => $"{_runPrefix}:work:queue";

    public string Results => $"{_runPrefix}:results";

    public string ResultChannel(string moduleTypeName) => $"{_runPrefix}:results:{moduleTypeName}";

    public string Workers => $"{_runPrefix}:workers";

    public string WorkerHeartbeatField(int workerIndex) => $"heartbeat:{workerIndex}";

    public string WorkAvailableChannel => $"{_runPrefix}:work:available";

    public string CompletionFlag => $"{_runPrefix}:completion";

    public string CompletionChannel => $"{_runPrefix}:completion:signal";

    public string CancellationFlag => $"{_runPrefix}:cancellation";

    public string CancellationChannel => $"{_runPrefix}:cancellation:signal";

    // Artifact keys
    public string ArtifactMeta(string artifactId) => $"{_runPrefix}:artifacts:meta:{artifactId}";

    public string ArtifactData(string artifactId) => $"{_runPrefix}:artifacts:data:{artifactId}";

    public string ArtifactChunk(string artifactId, int chunkIndex) => $"{_runPrefix}:artifacts:data:{artifactId}:chunk:{chunkIndex}";

    public string ArtifactIndex(string moduleTypeName) => $"{_runPrefix}:artifacts:index:{moduleTypeName}";

    /// <summary>
    /// Returns all non-channel keys (for setting expiration).
    /// </summary>
    public IEnumerable<string> AllStorageKeys =>
    [
        WorkQueue,
        Results,
        Workers,
        CompletionFlag,
        CancellationFlag,
    ];
}
