using System.Text;
using Respire;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Configuration shared by every Redis-backed feature in this package: the distributed
/// coordinator, the distributed artifact store and the Redis module cache.
/// </summary>
public sealed record RedisOptions
{
    /// <summary>
    /// Gets the Respire connection string: <c>host:port</c>, a <c>redis://</c> or <c>rediss://</c>
    /// URI, or a StackExchange.Redis-style comma-delimited string such as
    /// <c>localhost:6379,password=secret,ssl=true</c>. Unknown options, such as <c>abortConnect</c>,
    /// are rejected. Required unless <see cref="ConfigureConnection"/> supplies the endpoints.
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;

    /// <summary>
    /// Gets a function that transforms the parsed <see cref="RespireOptions"/> before startup
    /// validation. It runs once per materialized options instance, after all configuration and
    /// post-configuration callbacks. The resulting snapshot must contain at least one endpoint and is
    /// reused for lazy connections and retries. Use it for passwords containing commas, TLS, and
    /// timeouts. Later changes to these options do not change that snapshot.
    /// </summary>
    public Func<RespireOptions, RespireOptions>? ConfigureConnection { get; init; }

    /// <summary>
    /// Gets the prefix for every Redis key. Run state and artifacts live under
    /// <c>{KeyPrefix}:{runId}</c> and module cache entries under <c>{KeyPrefix}:module-cache</c>.
    /// Default: <c>modpipe</c>.
    /// </summary>
    public string KeyPrefix { get; init; } = "modpipe";

    /// <summary>
    /// Gets how long Redis keys live without renewal. The master's heartbeat renews distributed run
    /// keys and artifact keys while the run is active. This must exceed
    /// <see cref="DistributedOptions.ModuleResultTimeout"/> when the coordinator or artifact store is
    /// registered. An infinite module result timeout does not disable key expiration. Standalone artifact
    /// stores without a master coordinator retain artifacts only for this period. Default: 1 hour.
    /// </summary>
    public TimeSpan TimeToLive { get; init; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Gets the size of each Redis value used to store artifacts and module cache entries.
    /// Content that fits in one chunk is stored under a single key. Each chunk is one command, so it
    /// must transfer within the connection's <see cref="RespireOptions.CommandTimeout"/> even when
    /// several processes share the link. Default: 1 MB.
    /// </summary>
    public int ChunkSizeBytes { get; init; } = 1024 * 1024;

    /// <summary>
    /// Gets whether coordinator and artifact store startup fails when INFO memory reports a finite
    /// maxmemory with an evicting policy, or when diagnostics cannot establish the configuration.
    /// Default: false, which logs a warning and continues. INFO permission is needed for this check.
    /// The check observes the server answering INFO, not every node in a Redis Cluster, and does not
    /// guard against configuration changes after startup. Module cache connections are not checked.
    /// </summary>
    public bool FailOnUnsafeMemoryPolicy { get; init; }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("ConnectionString = <redacted>");
        builder.Append(", KeyPrefix = ").Append(KeyPrefix);
        builder.Append(", TimeToLive = ").Append(TimeToLive);
        builder.Append(", ChunkSizeBytes = ").Append(ChunkSizeBytes);
        return true;
    }
}
