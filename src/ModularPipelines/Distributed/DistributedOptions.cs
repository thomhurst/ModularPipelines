namespace ModularPipelines.Distributed;

/// <summary>
/// Configures distributed pipeline execution.
/// </summary>
public class DistributedOptions
{
    internal bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets this instance's distributed role. <see cref="DistributedRole.Auto"/> derives
    /// the role from <see cref="InstanceIndex"/>.
    /// </summary>
    public DistributedRole Role { get; set; } = DistributedRole.Auto;

    /// <summary>
    /// Gets or sets this process's zero-based index. It must be less than <see cref="TotalInstances"/>.
    /// Instance zero is the master when <see cref="Role"/> is <see cref="DistributedRole.Auto"/>, and
    /// every process derives its <see cref="Distributed.WorkerId"/> from its index.
    /// </summary>
    public int InstanceIndex { get; set; }

    /// <summary>
    /// Gets or sets the number of processes (master and workers) that take part in the run.
    /// A value greater than one requires a shared coordinator backend.
    /// </summary>
    public int TotalInstances { get; set; } = 1;

    /// <summary>
    /// Gets or sets an optional per-node limit for concurrently executing distributed modules.
    /// When unset, the pipeline's global concurrency limit is used. A configured value can
    /// lower, but cannot raise, that global limit.
    /// </summary>
    public int? MaxParallelism { get; set; }

    /// <summary>
    /// Gets or sets the identifier shared by every process in this pipeline run.
    /// The options pipeline resolves an empty value from <c>MODULARPIPELINES_RUN_ID</c>, or generates
    /// an identifier for a single-instance run when <see cref="RequireExplicitRunId"/> is false.
    /// Run identifiers contain 1-128 ASCII letters, digits, <c>.</c>, <c>_</c> or <c>-</c>, because
    /// backends embed them in keys and URLs.
    /// </summary>
    public string RunId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this run must provide <see cref="RunId"/> explicitly or through
    /// <c>MODULARPIPELINES_RUN_ID</c>. Shared backends enable this automatically.
    /// </summary>
    public bool RequireExplicitRunId { get; set; }

    /// <summary>
    /// Gets or sets capabilities this instance advertises in addition to those detected by registered
    /// <see cref="ICapabilityProvider"/> services. The current operating system is detected automatically.
    /// Use <see cref="CapabilityPipelineBuilderExtensions.AddCapabilities"/> to declare capabilities
    /// without enabling distributed mode.
    /// </summary>
    public IReadOnlyList<Capability> Capabilities { get; set; } = [];

    /// <summary>
    /// Gets or sets how long the master waits for workers to register: both for
    /// <see cref="MinimumWorkerCount"/> before dispatch starts and, for each assignment the master
    /// cannot run itself, for a worker with the required capabilities. Defaults to 5 minutes.
    /// </summary>
    public TimeSpan WorkerRegistrationTimeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets how often workers report liveness and renew the leases on their in-flight modules.
    /// Defaults to 5 seconds.
    /// </summary>
    public TimeSpan WorkerHeartbeatInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets how long a worker registration and its module leases remain valid without a
    /// heartbeat. When a lease expires the master returns its module to the queue. Must exceed
    /// <see cref="WorkerHeartbeatInterval"/>. Defaults to 30 seconds.
    /// </summary>
    public TimeSpan WorkerTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets how long workers keep running without a sign of life from a master that has not
    /// signalled completion, or without being able to check on the master at all. After this period
    /// the worker cancels its in-flight modules and fails, so a master that exited, failed or crashed
    /// does not leave workers running. Must exceed <see cref="WorkerHeartbeatInterval"/>. Defaults to
    /// 1 minute.
    /// </summary>
    /// <remarks>
    /// Backends that hold a connection to the master, such as SignalR, detect a lost master when
    /// that connection closes for good and do not wait for this period.
    /// </remarks>
    public TimeSpan MasterTimeout { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Gets or sets the minimum number of external workers that must register before
    /// the master starts dispatching work. The default is zero, which starts dispatching immediately.
    /// Waiting stops when <see cref="WorkerRegistrationTimeout"/> expires, after which dispatch proceeds
    /// with the workers currently available even when the configured minimum was not reached.
    /// </summary>
    public int MinimumWorkerCount { get; set; }

    /// <summary>
    /// Gets or sets how long the master waits for a claimed module's result beyond the module's own
    /// timeouts. The worker enforces the module's per-attempt timeout; the master's deadline starts
    /// when a worker claims the module and allows every configured attempt plus this period.
    /// Defaults to 45 minutes. Set to <see cref="TimeSpan.Zero"/> to wait indefinitely.
    /// </summary>
    public TimeSpan ModuleResultTimeout { get; set; } = TimeSpan.FromMinutes(45);

    /// <summary>
    /// Gets the identifier this process uses when it registers, claims work and publishes results.
    /// </summary>
    internal WorkerId LocalWorkerId => WorkerId.FromInstanceIndex(InstanceIndex);
}
