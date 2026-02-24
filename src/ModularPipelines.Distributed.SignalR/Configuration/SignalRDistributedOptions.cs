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
    /// Connection timeout in seconds for worker connections to the master.
    /// </summary>
    public int ConnectionTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Whether workers should automatically reconnect on connection loss.
    /// </summary>
    public bool EnableAutoReconnect { get; set; } = true;

    /// <summary>
    /// Maximum number of reconnect attempts before giving up.
    /// </summary>
    public int MaxReconnectAttempts { get; set; } = 5;

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
    /// Timeout in seconds for the tunnel to start and provide a public URL.
    /// </summary>
    public int TunnelStartupTimeoutSeconds { get; set; } = 30;
}
