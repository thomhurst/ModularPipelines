using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Configuration options for Redis-based master endpoint discovery.
/// </summary>
public sealed record RedisDiscoveryOptions
{
    private static readonly ConditionalWeakTable<RedisDiscoveryOptions, Lazy<ConfigurationOptions>> Configurations = [];

    /// <summary>
    /// Gets the Redis connection string. Defaults to empty; configure it explicitly unless
    /// <see cref="ConfigureConnection"/> supplies endpoints or both <see cref="RestUrl"/> and <see cref="RestToken"/> are set.
    /// Discovery opens its own connection; it does not use or replace a connection registered by the application.
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;

    /// <summary>
    /// Gets an optional callback that adjusts the connection configuration parsed from
    /// <see cref="ConnectionString"/>, for example to set credentials or TLS options.
    /// When the connection string is empty, the callback must supply the endpoints.
    /// Runs during options validation for TCP discovery; a defensive snapshot is reused when connecting.
    /// Later changes to the callback's argument do not affect the validated configuration.
    /// REST discovery does not invoke this callback.
    /// </summary>
    public Action<ConfigurationOptions>? ConfigureConnection { get; init; }

    /// <summary>
    /// Gets the optional absolute HTTPS Upstash Redis REST URL. HTTP is supported only on loopback. Set this together with <see cref="RestToken"/>
    /// to use HTTP instead of the Redis TCP protocol. Blank values are treated as omitted.
    /// Path prefixes are supported; query strings and fragments are not.
    /// </summary>
    public Uri? RestUrl
    {
        get;
        init => field = string.IsNullOrWhiteSpace(value?.OriginalString) ? null : value;
    }

    /// <summary>
    /// Gets the optional Upstash Redis REST token. Set this together with <see cref="RestUrl"/>.
    /// </summary>
    public string? RestToken { get; init; }

    /// <summary>
    /// Gets the prefix for the Redis keys used by discovery. Default: <c>modpipe</c>.
    /// </summary>
    public string KeyPrefix { get; init; } = "modpipe";

    /// <summary>
    /// Gets how long the advertised endpoint remains stored. Prevents stale endpoints from
    /// persisting. Default: 1 hour.
    /// </summary>
    public TimeSpan TimeToLive { get; init; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Gets how long workers wait to discover the master endpoint. Default: 2 minutes.
    /// </summary>
    public TimeSpan DiscoveryTimeout { get; init; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets how often workers check for the master endpoint. Default: 500 milliseconds.
    /// </summary>
    public TimeSpan PollInterval { get; init; } = TimeSpan.FromMilliseconds(500);

    internal ConfigurationOptions GetConnectionConfiguration()
    {
        var configuration = Configurations.GetValue(this, static options => new Lazy<ConfigurationOptions>(() =>
        {
            var parsed = string.IsNullOrWhiteSpace(options.ConnectionString)
                ? new ConfigurationOptions()
                : ConfigurationOptions.Parse(options.ConnectionString);
            options.ConfigureConnection?.Invoke(parsed);
            return CloneConfiguration(parsed);
        })).Value;
        return CloneConfiguration(configuration);
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
