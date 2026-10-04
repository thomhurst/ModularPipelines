namespace ModularPipelines.Distributed.SignalR;

/// <summary>
/// Configuration options for the SignalR-based distributed coordinator.
/// </summary>
public class SignalRDistributedOptions
{
    /// <summary>
    /// Gets or sets the absolute HTTP or HTTPS URL the master binds its SignalR server to. Use port 0 to let the operating
    /// system choose a port. Default: <c>http://localhost:5099</c>.
    /// </summary>
    public Uri ListenUrl { get; set; } = new("http://localhost:5099");

    /// <summary>
    /// Gets or sets the URL workers use to reach the master. The master advertises it through
    /// <see cref="IMasterDiscovery"/>, and workers without discovery connect to it. When unset, the
    /// master advertises the tunnel URL or the bound listen URL, and workers without discovery
    /// connect to <see cref="ListenUrl"/>.
    /// </summary>
    public Uri? AdvertisedUrl { get; set; }

    /// <summary>
    /// Gets or sets the hub path for the SignalR pipeline hub. Default: <c>/pipeline-hub</c>.
    /// </summary>
    public string HubPath { get; set; } = "/pipeline-hub";

    /// <summary>
    /// Gets or sets the access token workers present to the master. When unset, the master generates
    /// a token whenever it is reachable beyond the local machine (a tunnel or a non-loopback
    /// <see cref="ListenUrl"/>) and shares it through <see cref="IMasterDiscovery"/>; without discovery
    /// such a master fails at startup until a token is configured on every process. The token is
    /// never logged.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets how long a worker keeps trying to connect to the master. Default: 2 minutes.
    /// </summary>
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets or sets how many times a worker reconnects after losing its connection, with exponential
    /// backoff. Zero disables reconnecting. A worker that stays disconnected for longer than
    /// <see cref="DistributedOptions.WorkerTimeout"/> loses its leases, and the master requeues its
    /// in-flight modules. Default: 5.
    /// </summary>
    public int MaxReconnectAttempts { get; set; } = 5;

    /// <summary>
    /// Gets or sets how often each side sends a keep-alive ping. Applied to the server's
    /// KeepAliveInterval and the worker connection's KeepAliveInterval. Default: 5 seconds.
    /// </summary>
    public TimeSpan KeepAliveInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets how long a side waits without any message before declaring the peer gone.
    /// Applied to the server's ClientTimeoutInterval and the worker connection's ServerTimeout.
    /// Must be at least twice <see cref="KeepAliveInterval"/>. Default: 15 seconds.
    /// </summary>
    public TimeSpan PeerTimeout { get; set; } = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Gets or sets the maximum size in bytes of one SignalR message, such as a module result.
    /// Default: 1 MiB.
    /// </summary>
    public long MaxMessageSizeBytes { get; set; } = 1024 * 1024;

    /// <summary>
    /// Gets the options for exposing the master through a cloudflared quick tunnel.
    /// </summary>
    public SignalRTunnelOptions Tunnel { get; set; } = new();
}

/// <summary>
/// Configures the cloudflared quick tunnel that can expose a SignalR master publicly.
/// </summary>
public class SignalRTunnelOptions
{
    /// <summary>
    /// Gets or sets whether the master starts a cloudflared tunnel and advertises its public URL.
    /// Requires <c>cloudflared</c> and an access token, which is generated when none is configured.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the path to the cloudflared binary. Default: <c>cloudflared</c> on PATH.
    /// </summary>
    public string CloudflaredPath { get; set; } = "cloudflared";

    /// <summary>
    /// Gets or sets how long to wait for the tunnel to report its public URL. Default: 30 seconds.
    /// </summary>
    public TimeSpan StartupTimeout { get; set; } = TimeSpan.FromSeconds(30);
}
