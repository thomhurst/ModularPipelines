namespace ModularPipelines.Distributed;

public class DistributedOptions
{
    internal bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets this instance's distributed role. <see cref="DistributedRole.Auto"/> derives
    /// the role from <see cref="InstanceIndex"/>.
    /// </summary>
    public DistributedRole Role { get; set; } = DistributedRole.Auto;

    public int InstanceIndex { get; set; }

    public int TotalInstances { get; set; } = 1;

    /// <summary>
    /// Gets or sets the identifier shared by every process in this pipeline run.
    /// The options pipeline resolves an empty value from <c>MODULARPIPELINES_RUN_ID</c>, or generates
    /// an identifier for a single-instance run when <see cref="RequireExplicitRunId"/> is false.
    /// </summary>
    public string RunId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this run must provide <see cref="RunId"/> explicitly or through
    /// <c>MODULARPIPELINES_RUN_ID</c>. Shared backends enable this automatically.
    /// </summary>
    public bool RequireExplicitRunId { get; set; }

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
