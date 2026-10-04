using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Configuration shared by every Redis-backed feature in this package: the distributed
/// coordinator, the distributed artifact store and the Redis module cache.
/// </summary>
public class RedisOptions
{
    /// <summary>
    /// Gets or sets the StackExchange.Redis connection string, such as <c>localhost:6379</c>.
    /// Required unless <see cref="ConfigureConnection"/> supplies the endpoints.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a callback that adjusts the parsed <see cref="ConfigurationOptions"/> before
    /// startup validation. It runs once per materialized options instance, after all configuration
    /// and post-configuration callbacks. The resulting snapshot must contain at least one endpoint
    /// and is reused for lazy connections and retries. Use it for passwords containing commas,
    /// TLS, and retry settings. Later changes to these options do not change that snapshot.
    /// </summary>
    public Action<ConfigurationOptions>? ConfigureConnection { get; set; }

    /// <summary>
    /// Gets or sets the prefix for every Redis key. Run state and artifacts live under
    /// <c>{KeyPrefix}:{runId}</c> and module cache entries under <c>{KeyPrefix}:module-cache</c>.
    /// Default: <c>modpipe</c>.
    /// </summary>
    public string KeyPrefix { get; set; } = "modpipe";

    /// <summary>
    /// Gets or sets how long Redis keys live. Distributed run keys and artifacts must outlive the
    /// run, so this must exceed <see cref="DistributedOptions.ModuleResultTimeout"/> when the
    /// coordinator or artifact store is registered. Default: 1 hour.
    /// </summary>
    public TimeSpan TimeToLive { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Gets or sets the size of each Redis value used to store artifacts and module cache entries.
    /// Content that fits in one chunk is stored under a single key. Each chunk is one command, so it
    /// must transfer within the connection's <see cref="ConfigurationOptions.AsyncTimeout"/> even when
    /// several processes share the link. Default: 1 MB.
    /// </summary>
    public int ChunkSizeBytes { get; set; } = 1024 * 1024;
}
