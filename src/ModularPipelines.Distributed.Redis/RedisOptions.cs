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
    /// connecting. Use it for values that a connection string cannot carry safely, such as a
    /// password containing commas, or for TLS and retry settings.
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
    /// Content that fits in one chunk is stored under a single key. Default: 4 MB.
    /// </summary>
    public int ChunkSizeBytes { get; set; } = 4 * 1024 * 1024;
}
