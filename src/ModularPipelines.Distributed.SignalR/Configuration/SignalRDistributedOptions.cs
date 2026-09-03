namespace ModularPipelines.Distributed.SignalR.Configuration;

/// <summary>
/// Configuration options for the SignalR-based distributed coordinator.
/// </summary>
public class SignalRDistributedOptions
{
    /// <summary>
    /// The URL the master will listen on. Workers connect to this URL.
    /// </summary>
    public string MasterUrl { get; set; } = "http://localhost:5099";

    /// <summary>
    /// The hub path for the SignalR pipeline hub.
    /// </summary>
    public string HubPath { get; set; } = "/pipeline-hub";

    /// <summary>
    /// Connection timeout for worker connections to the master. Default: 2 minutes.
    /// </summary>
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Whether workers should automatically reconnect on connection loss.
    /// </summary>
    public bool EnableAutoReconnect { get; set; } = true;

    /// <summary>
    /// Maximum number of reconnect attempts before giving up.
    /// </summary>
    public int MaxReconnectAttempts { get; set; } = 5;

    /// <summary>
    /// How long the master waits for a disconnected worker to reconnect before
    /// re-enqueuing its in-flight module. Should exceed the total auto-reconnect window
    /// (exponential backoff over <see cref="MaxReconnectAttempts"/>) so a transient blip
    /// doesn't cause the module to run twice. Default 45s.
    /// </summary>
    public TimeSpan ReconnectGrace { get; set; } = TimeSpan.FromSeconds(45);

    /// <summary>
    /// How often each side sends a keep-alive ping. Lower values let a
    /// silent connection drop (e.g. a crashed/partitioned worker whose socket close
    /// is masked by the tunnel) be detected sooner, so its in-flight work is
    /// re-queued faster. Applied to both the server's KeepAliveInterval and the
    /// worker connection's KeepAliveInterval. Default 5s (SignalR default is 15s).
    /// </summary>
    public TimeSpan KeepAliveInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// How long a side waits without any message before declaring the peer gone
    /// Applied to the server's ClientTimeoutInterval (how fast the master
    /// detects a dead worker and re-queues its work) and the worker connection's
    /// ServerTimeout (how fast a worker detects a dead master and reconnects).
    /// Should be at least twice <see cref="KeepAliveInterval"/>. Default 15s
    /// (SignalR default is 30s).
    /// </summary>
    public TimeSpan PeerTimeout { get; set; } = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Maximum size in bytes for a single SignalR message (default 1MB).
    /// Increase for large module results.
    /// </summary>
    public long MaximumReceiveMessageSize { get; set; } = 1024 * 1024;

    /// <summary>
    /// When true, the master starts a cloudflared tunnel to expose the SignalR server publicly.
    /// Workers connect via the tunnel URL instead of the local MasterUrl.
    /// Requires 'cloudflared' to be available on PATH.
    /// </summary>
    public bool EnableTunnel { get; set; }

    /// <summary>
    /// Path to the cloudflared binary. Defaults to "cloudflared" (on PATH).
    /// </summary>
    public string CloudflaredPath { get; set; } = "cloudflared";

    /// <summary>
    /// Timeout for the tunnel to start and provide a public URL. Default: 30 seconds.
    /// </summary>
    public TimeSpan TunnelStartupTimeout { get; set; } = TimeSpan.FromSeconds(30);
}
