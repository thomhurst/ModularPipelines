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
    /// Gets or sets an optional per-node limit for concurrently executing distributed modules.
    /// When unset, the pipeline's global concurrency limit is used. A configured value can
    /// lower, but cannot raise, that global limit.
    /// </summary>
    public int? MaxParallelism { get; set; }

    /// <summary>
    /// Gets or sets an identifier shared by every process in this pipeline execution.
    /// Coordinators should populate this from their execution-scoping identifier.
    /// </summary>
    public string? RunIdentifier { get; set; }

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

    /// <summary>
    /// Gets or sets the minimum number of external workers that must register before
    /// the master starts dispatching work. The default is zero, which starts dispatching immediately.
    /// Waiting stops when <see cref="CapabilityTimeout"/> expires, after which dispatch proceeds
    /// with the workers currently available even when the configured minimum was not reached.
    /// </summary>
    public int MinimumWorkerCount { get; set; }

    public bool AutoDetectOsCapability { get; set; } = true;

    /// <summary>
    /// Gets or sets the default timeout for waiting for a distributed module result.
    /// Defaults to 45 minutes and applies when a module has no explicit Timeout configured.
    /// Set to <see cref="TimeSpan.Zero"/> to wait indefinitely.
    /// </summary>
    public TimeSpan ModuleResultTimeout { get; set; } = TimeSpan.FromMinutes(45);
}
