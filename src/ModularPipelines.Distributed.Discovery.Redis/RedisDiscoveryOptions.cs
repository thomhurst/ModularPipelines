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
    /// Key prefix for all Redis keys used by the discovery mechanism.
    /// </summary>
    public string KeyPrefix { get; set; } = "modular-pipelines";

    /// <summary>
    /// Optional run identifier for isolating concurrent pipeline runs.
    /// If null, uses a hash of the current working directory.
    /// </summary>
    public string? RunIdentifier { get; set; }

    /// <summary>
    /// TTL in seconds for the master URL key. Prevents stale URLs from persisting.
    /// </summary>
    public int TtlSeconds { get; set; } = 3600;

    /// <summary>
    /// Timeout in seconds for workers waiting to discover the master URL.
    /// </summary>
    public int DiscoveryTimeoutSeconds { get; set; } = 120;

    /// <summary>
    /// Poll interval in milliseconds for workers checking for master URL availability.
    /// </summary>
    public int PollIntervalMs { get; set; } = 500;
}
