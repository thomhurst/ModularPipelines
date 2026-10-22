using System.Text;

namespace ModularPipelines.Distributed.SignalR;

/// <summary>
/// Configuration options for the SignalR-based distributed coordinator.
/// </summary>
public sealed record SignalRDistributedOptions
{
    /// <summary>
    /// Gets the absolute HTTP or HTTPS URL the master binds its SignalR server to. Use port 0 to let the operating
    /// system choose a port. Default: <c>http://localhost:5099</c>.
    /// </summary>
    public Uri ListenUrl { get; init; } = new("http://localhost:5099");

    /// <summary>
    /// Gets the URL workers use to reach the master. The master advertises it through
    /// <see cref="IMasterDiscovery"/>, and workers without discovery connect to it. When unset, the
    /// master advertises the tunnel URL or the bound listen URL, and workers without discovery
    /// connect to <see cref="ListenUrl"/>. Workers preserve any path prefix in the endpoint URL when
    /// appending <see cref="HubPath"/>.
    /// </summary>
    public Uri? AdvertisedUrl { get; init; }

    /// <summary>
    /// Gets the hub path for the SignalR pipeline hub. Default: <c>/pipeline-hub</c>.
    /// </summary>
    public string HubPath { get; init; } = "/pipeline-hub";

    /// <summary>
    /// Gets the access token workers present to the master. When unset, the master generates
    /// a token whenever it is reachable beyond the local machine (a tunnel or a non-loopback
    /// <see cref="ListenUrl"/>) and shares it through <see cref="IMasterDiscovery"/>; without discovery
    /// such a master fails at startup until a token is configured on every process. The token is
    /// never logged.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    /// Gets how long a worker keeps trying to connect to the master. Default: 2 minutes.
    /// </summary>
    public TimeSpan ConnectionTimeout { get; init; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets how many times a worker reconnects after losing its connection, with exponential
    /// backoff. Zero disables reconnecting. Recovery, including connection attempts and worker
    /// registration, is also bounded by <see cref="DistributedOptions.WorkerTimeout"/> minus
    /// <see cref="PeerTimeout"/> and <see cref="DistributedOptions.WorkerHeartbeatInterval"/>.
    /// Reconnecting is disabled when those reservations leave no time. A worker whose leases
    /// expire loses them, and the master requeues its in-flight modules. Default: 5.
    /// </summary>
    public int MaxReconnectAttempts { get; init; } = 5;

    /// <summary>
    /// Gets how often each side sends a keep-alive ping. Applied to the server's
    /// KeepAliveInterval and the worker connection's KeepAliveInterval. Default: 5 seconds.
    /// </summary>
    public TimeSpan KeepAliveInterval { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets how long a side waits without any message before declaring the peer gone.
    /// Applied to the server's ClientTimeoutInterval and the worker connection's ServerTimeout.
    /// Must be at least twice <see cref="KeepAliveInterval"/>. Default: 15 seconds.
    /// </summary>
    public TimeSpan PeerTimeout { get; init; } = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Gets the maximum size in bytes of one SignalR message, such as a module result.
    /// Includes JSON escaping and invocation overhead. Workers replace oversized results with a
    /// failure that fits this limit; a limit too small for the failure is rejected locally.
    /// Configure the same limit on every process. Default: 1 MiB.
    /// </summary>
    public long MaxMessageSizeBytes { get; init; } = 1024 * 1024;

    /// <summary>
    /// Gets the options for exposing the master through a cloudflared quick tunnel.
    /// </summary>
    public SignalRTunnelOptions Tunnel { get; init; } = new();

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("ListenUrl = ").Append(ListenUrl is null ? "<none>" : "<redacted>");
        builder.Append(", AdvertisedUrl = ").Append(AdvertisedUrl is null ? "<none>" : "<redacted>");
        builder.Append(", HubPath = ").Append(HubPath);
        builder.Append(", AccessToken = ").Append(AccessToken is null ? "<none>" : "<redacted>");
        builder.Append(", ConnectionTimeout = ").Append(ConnectionTimeout);
        builder.Append(", MaxReconnectAttempts = ").Append(MaxReconnectAttempts);
        builder.Append(", KeepAliveInterval = ").Append(KeepAliveInterval);
        builder.Append(", PeerTimeout = ").Append(PeerTimeout);
        builder.Append(", MaxMessageSizeBytes = ").Append(MaxMessageSizeBytes);
        builder.Append(", Tunnel = ").Append(Tunnel);
        return true;
    }
}

/// <summary>
/// Configures the cloudflared quick tunnel that can expose a SignalR master publicly.
/// </summary>
public sealed record SignalRTunnelOptions
{
    /// <summary>
    /// Gets whether the master starts a cloudflared tunnel and advertises its public URL.
    /// Requires <c>cloudflared</c> and an access token, which is generated when none is configured.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Gets the path to the cloudflared binary. Default: <c>cloudflared</c> on PATH.
    /// </summary>
    public string CloudflaredPath { get; init; } = "cloudflared";

    /// <summary>
    /// Gets how long to wait for the tunnel to report its public URL. Default: 30 seconds.
    /// </summary>
    public TimeSpan StartupTimeout { get; init; } = TimeSpan.FromSeconds(30);
}
