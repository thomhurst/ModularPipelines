using ModularPipelines.Distributed.Configuration;

namespace ModularPipelines.Distributed;

public class DistributedOptions
{
    public bool Enabled { get; set; }

    public int InstanceIndex { get; set; }

    public int TotalInstances { get; set; } = 1;

    /// <summary>
    /// Gets or sets the identifier shared by every process in this pipeline run.
    /// Defaults to <c>RUN_IDENTIFIER</c> when set; otherwise a new identifier is generated.
    /// </summary>
    public string RunId { get; set; } = RunIdResolver.Resolve(null);

    public IReadOnlyList<Capability> Capabilities { get; set; } = [];

    public TimeSpan CapabilityTimeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets how often workers report liveness to the coordinator.
    /// </summary>
    public TimeSpan WorkerHeartbeatInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets how long a worker remains live without a heartbeat.
    /// </summary>
    public TimeSpan WorkerTimeout { get; set; } = TimeSpan.FromSeconds(30);

    public bool AutoDetectOsCapability { get; set; } = true;

    /// <summary>
    /// Gets or sets the default timeout for waiting for a distributed module result.
    /// Defaults to 45 minutes and applies when a module has no explicit Timeout configured.
    /// Set to <see cref="TimeSpan.Zero"/> to wait indefinitely.
    /// </summary>
    public TimeSpan ModuleResultTimeout { get; set; } = TimeSpan.FromMinutes(45);
}
