using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Configuration options for Redis-based master endpoint discovery.
/// </summary>
public class RedisDiscoveryOptions
{
    /// <summary>
    /// Gets or sets the Redis connection string. Required unless <see cref="RestUrl"/> is set.
    /// Discovery opens its own connection; it does not use or replace a connection registered by the application.
    /// </summary>
    public string ConnectionString { get; set; } = "localhost:6379";

    /// <summary>
    /// Gets or sets an optional callback that adjusts the connection configuration parsed from
    /// <see cref="ConnectionString"/>, for example to set credentials or TLS options.
    /// </summary>
    public Action<ConfigurationOptions>? ConfigureConnection { get; set; }

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
    /// Gets or sets the prefix for the Redis keys used by discovery. Default: <c>modpipe</c>.
    /// </summary>
    public string KeyPrefix { get; set; } = "modpipe";

    /// <summary>
    /// Gets or sets how long the advertised endpoint remains stored. Prevents stale endpoints from
    /// persisting. Default: 1 hour.
    /// </summary>
    public TimeSpan TimeToLive { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Gets or sets how long workers wait to discover the master endpoint. Default: 2 minutes.
    /// </summary>
    public TimeSpan DiscoveryTimeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets or sets how often workers check for the master endpoint. Default: 500 milliseconds.
    /// </summary>
    public TimeSpan PollInterval { get; set; } = TimeSpan.FromMilliseconds(500);
}
