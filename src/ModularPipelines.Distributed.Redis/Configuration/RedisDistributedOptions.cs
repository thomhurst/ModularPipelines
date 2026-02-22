namespace ModularPipelines.Distributed.Redis.Configuration;

/// <summary>
/// Configuration options for the Redis distributed coordinator.
/// </summary>
public class RedisDistributedOptions
{
    /// <summary>
    /// Gets or sets the Redis connection string. Required.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the run identifier used to isolate keys across concurrent pipeline runs.
    /// If not set, auto-detected from CI environment variables or git commit SHA.
    /// </summary>
    public string? RunIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the prefix for all Redis keys. Default: "modpipe".
    /// </summary>
    public string KeyPrefix { get; set; } = "modpipe";

    /// <summary>
    /// Gets or sets the TTL in seconds for Redis keys. Default: 3600 (1 hour).
    /// </summary>
    public int KeyExpirationSeconds { get; set; } = 3600;

    /// <summary>
    /// Gets or sets the delay in milliseconds between dequeue poll attempts. Default: 100.
    /// </summary>
    public int DequeuePollDelayMilliseconds { get; set; } = 100;
}
