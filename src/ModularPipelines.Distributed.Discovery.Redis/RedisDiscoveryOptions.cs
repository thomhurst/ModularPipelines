namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Configuration options for Redis-based master URL discovery.
/// </summary>
public class RedisDiscoveryOptions
{
    /// <summary>
    /// Redis connection string.
    /// </summary>
    public string ConnectionString { get; set; } = "localhost:6379";

    /// <summary>
    /// Gets or sets the optional Upstash Redis REST URL. Set this together with <see cref="RestToken"/>
    /// to use HTTP instead of the Redis TCP protocol.
    /// </summary>
    public string? RestUrl { get; set; }

    /// <summary>
    /// Gets or sets the optional Upstash Redis REST token. Set this together with <see cref="RestUrl"/>.
    /// </summary>
    public string? RestToken { get; set; }

    /// <summary>
    /// Key prefix for all Redis keys used by the discovery mechanism.
    /// </summary>
    public string KeyPrefix { get; set; } = "modular-pipelines";

    /// <summary>
    /// Optional run identifier for isolating concurrent pipeline runs.
    /// If null, uses the current CI commit SHA, local Git SHA, or a unique fallback.
    /// </summary>
    public string? RunIdentifier { get; set; }

    /// <summary>
    /// TTL for the master URL key. Prevents stale URLs from persisting. Default: 1 hour.
    /// </summary>
    public TimeSpan Ttl { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Timeout for workers waiting to discover the master URL. Default: 2 minutes.
    /// </summary>
    public TimeSpan DiscoveryTimeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Poll interval for workers checking for master URL availability. Default: 500 milliseconds.
    /// </summary>
    public TimeSpan PollInterval { get; set; } = TimeSpan.FromMilliseconds(500);
}
