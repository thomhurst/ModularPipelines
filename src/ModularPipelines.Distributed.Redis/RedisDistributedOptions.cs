namespace ModularPipelines.Distributed.Redis;

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
    /// Gets or sets the prefix for all Redis keys. Default: "modpipe".
    /// </summary>
    public string KeyPrefix { get; set; } = "modpipe";

    /// <summary>
    /// Gets or sets the TTL in seconds for Redis keys. Default: 3600 (1 hour).
    /// </summary>
    public int KeyExpirationSeconds { get; set; } = 3600;
}
