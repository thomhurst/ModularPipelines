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
    /// Distributed coordination requires a unique value per execution. If not set, an
    /// invocation-scoped CI identifier is used when available.
    /// </summary>
    public string? RunIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the prefix for all Redis keys. Default: "modpipe".
    /// </summary>
    public string KeyPrefix { get; set; } = "modpipe";

    /// <summary>
    /// Gets or sets the TTL for Redis keys. Default: 1 hour.
    /// </summary>
    public TimeSpan KeyExpiration { get; set; } = TimeSpan.FromHours(1);
}
