using System.Net;
using System.Net.Sockets;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Configuration options for Redis-based master endpoint discovery.
/// </summary>
public class RedisDiscoveryOptions
{
    private readonly Lock _configurationLock = new();
    private ConfigurationOptions? _connectionConfiguration;

    /// <summary>
    /// Gets or sets the Redis connection string. Defaults to empty; configure it explicitly unless
    /// <see cref="ConfigureConnection"/> supplies endpoints or both <see cref="RestUrl"/> and <see cref="RestToken"/> are set.
    /// Discovery opens its own connection; it does not use or replace a connection registered by the application.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional callback that adjusts the connection configuration parsed from
    /// <see cref="ConnectionString"/>, for example to set credentials or TLS options.
    /// When the connection string is empty, the callback must supply the endpoints.
    /// Runs during options validation for TCP discovery; a defensive snapshot is reused when connecting.
    /// Later changes to the callback's argument do not affect the validated configuration.
    /// REST discovery does not invoke this callback.
    /// </summary>
    public Action<ConfigurationOptions>? ConfigureConnection { get; set; }

    /// <summary>
    /// Gets or sets the optional absolute HTTPS Upstash Redis REST URL. HTTP is supported only on loopback. Set this together with <see cref="RestToken"/>
    /// to use HTTP instead of the Redis TCP protocol. Blank values are treated as omitted.
    /// Path prefixes are supported; query strings and fragments are not.
    /// </summary>
    public Uri? RestUrl
    {
        get;
        set => field = string.IsNullOrWhiteSpace(value?.OriginalString) ? null : value;
    }

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

    internal ConfigurationOptions GetConnectionConfiguration()
    {
        lock (_configurationLock)
        {
            if (_connectionConfiguration is null)
            {
                var configuration = string.IsNullOrWhiteSpace(ConnectionString)
                    ? new ConfigurationOptions()
                    : ConfigurationOptions.Parse(ConnectionString);
                ConfigureConnection?.Invoke(configuration);
                _connectionConfiguration = CloneConfiguration(configuration);
            }

            return CloneConfiguration(_connectionConfiguration);
        }
    }

    private static ConfigurationOptions CloneConfiguration(ConfigurationOptions configuration)
    {
        var clone = configuration.Clone();
        // StackExchange.Redis copies the collection but shares mutable IP endpoints and addresses.
        for (var i = 0; i < clone.EndPoints.Count; i++)
        {
            if (clone.EndPoints[i] is IPEndPoint endpoint)
            {
                var address = endpoint.Address;
                var addressCopy = address.AddressFamily == AddressFamily.InterNetworkV6
                    ? new IPAddress(address.GetAddressBytes(), address.ScopeId)
                    : new IPAddress(address.GetAddressBytes());
                clone.EndPoints[i] = new IPEndPoint(addressCopy, endpoint.Port);
            }
        }

        return clone;
    }
}
