using System.Runtime.CompilerServices;
using System.Text;
using Respire;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Configuration options for Redis-based master endpoint discovery.
/// </summary>
public sealed record RedisDiscoveryOptions
{
    private static readonly ConditionalWeakTable<RedisDiscoveryOptions, Lazy<RespireOptions>> Configurations = [];

    /// <summary>
    /// Gets the Respire connection string: <c>host:port</c>, a <c>redis://</c> or <c>rediss://</c> URI, or a
    /// StackExchange.Redis-style comma-delimited string. Unknown options, such as <c>abortConnect</c>, are rejected.
    /// Defaults to empty; configure it explicitly unless
    /// <see cref="ConfigureConnection"/> supplies endpoints or both <see cref="RestUrl"/> and <see cref="RestToken"/> are set.
    /// Discovery opens its own connection; it does not use or replace a connection registered by the application.
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;

    /// <summary>
    /// Gets an optional function that transforms the <see cref="RespireOptions"/> parsed from
    /// <see cref="ConnectionString"/>, for example to set credentials or TLS options.
    /// When the connection string is empty, the function must supply the endpoints.
    /// Runs during options validation for TCP discovery; a defensive snapshot is reused when connecting.
    /// Later changes to an endpoint list the function kept do not affect the validated configuration.
    /// REST discovery does not invoke this function.
    /// </summary>
    /// <remarks>
    /// This beta package intentionally exposes Respire's connection options so callers can use its full
    /// configuration surface. Respire is pre-1.0; upgrading this package may require changes to this
    /// callback.
    /// </remarks>
    public Func<RespireOptions, RespireOptions>? ConfigureConnection { get; init; }

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
    /// Gets the expiry applied to the advertised endpoint. Renewed every third of this duration
    /// (at most one minute between renewals) while discovery is active, so late workers can join long runs.
    /// Cleanup stops renewal and removes this master's endpoint; disposal stops renewal without deleting it.
    /// After a crash or disposal without cleanup, the endpoint expires within this duration.
    /// Must be at least one second. Default: 1 hour.
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

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("ConnectionString = <redacted>");
        builder.Append(", RestUrl = ").Append(RestUrl is null ? "<none>" : "<redacted>");
        builder.Append(", RestToken = ").Append(RestToken is null ? "<none>" : "<redacted>");
        builder.Append(", KeyPrefix = ").Append(KeyPrefix);
        builder.Append(", TimeToLive = ").Append(TimeToLive);
        builder.Append(", DiscoveryTimeout = ").Append(DiscoveryTimeout);
        builder.Append(", PollInterval = ").Append(PollInterval);
        return true;
    }

    internal RespireOptions GetConnectionConfiguration()
    {
        var configuration = Configurations.GetValue(this, static options => new Lazy<RespireOptions>(() =>
        {
            var parsed = string.IsNullOrWhiteSpace(options.ConnectionString)
                ? new RespireOptions()
                : RespireOptions.Parse(options.ConnectionString);

            // After a dropped connection, Respire fails the next command at once while it reconnects. The default
            // retry policy resends commands that were never written, and reads. ConfigureConnection can replace it.
            parsed = parsed with { RetryPolicy = parsed.RetryPolicy ?? new RespireRetryPolicy() };
            if (options.ConfigureConnection is { } configure)
            {
                parsed = configure(parsed)
                    ?? throw new InvalidOperationException(
                        $"{nameof(RedisDiscoveryOptions)}.{nameof(ConfigureConnection)} must not return null.");
            }

            return CloneConfiguration(parsed);
        })).Value;
        return CloneConfiguration(configuration);
    }

    // RespireOptions is an immutable record except for its endpoint lists, which a copy would share.
    private static RespireOptions CloneConfiguration(RespireOptions configuration) => configuration with
    {
        Endpoints = [.. configuration.Endpoints],
        ReplicaEndpoints = [.. configuration.ReplicaEndpoints],
    };
}
