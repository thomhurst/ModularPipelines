using System.IO.Compression;

namespace ModularPipelines.Distributed;

/// <summary>
/// Configures distributed pipeline execution.
/// </summary>
public sealed record DistributedOptions
{
    /// <summary>
    /// Gets the compression level for directory and glob artifacts across all artifact stores,
    /// including in local mode. Defaults to <see cref="CompressionLevel.Fastest"/>.
    /// </summary>
    public CompressionLevel ArtifactCompressionLevel { get; init; } = CompressionLevel.Fastest;

    internal bool Enabled { get; init; }

    /// <summary>
    /// Gets this instance's distributed role. <see cref="DistributedRole.Auto"/> derives
    /// the role from <see cref="InstanceIndex"/>.
    /// </summary>
    public DistributedRole Role { get; init; } = DistributedRole.Auto;

    /// <summary>
    /// Gets this process's zero-based index. It must be less than <see cref="TotalInstances"/>.
    /// Instance zero is the master when <see cref="Role"/> is <see cref="DistributedRole.Auto"/>, and
    /// every process derives its <see cref="Distributed.WorkerId"/> from its index.
    /// </summary>
    public int InstanceIndex { get; init; }

    /// <summary>
    /// Gets the number of processes (master and workers) that take part in the run.
    /// A value greater than one requires a shared coordinator backend.
    /// </summary>
    public int TotalInstances { get; init; } = 1;

    /// <summary>
    /// Gets an optional per-node limit for concurrently executing distributed modules.
    /// When unset, the pipeline's global concurrency limit is used. A configured value can
    /// lower, but cannot raise, that global limit.
    /// </summary>
    public int? MaxParallelism { get; init; }

    /// <summary>
    /// Gets the identifier shared by every process in this pipeline run.
    /// The options pipeline resolves an empty value from <c>MODULARPIPELINES_RUN_ID</c>, or generates
    /// an identifier for a single-instance run when <see cref="RequireExplicitRunId"/> is false.
    /// Run identifiers contain 1-128 ASCII letters, digits, <c>.</c>, <c>_</c> or <c>-</c>, because
    /// backends embed them in keys and URLs.
    /// </summary>
    public string RunId { get; init; } = string.Empty;

    /// <summary>
    /// Gets whether this run must provide <see cref="RunId"/> explicitly or through
    /// <c>MODULARPIPELINES_RUN_ID</c>. Shared backends enable this automatically.
    /// </summary>
    public bool RequireExplicitRunId { get; init; }

    /// <summary>
    /// Gets capabilities this instance advertises in addition to those detected by registered
    /// <see cref="ICapabilityProvider"/> services. The current operating system is detected automatically.
    /// Use <see cref="CapabilityPipelineBuilderExtensions.AddCapabilities"/> to declare capabilities
    /// without enabling distributed mode.
    /// </summary>
    public IReadOnlyList<Capability> Capabilities { get; init; } = [];

    /// <summary>
    /// Gets how long the master waits for workers to register: both for
    /// <see cref="MinimumWorkerCount"/> before dispatch starts and, for each assignment the master
    /// cannot run itself, for a worker with the required capabilities. Defaults to 5 minutes.
    /// </summary>
    public TimeSpan WorkerRegistrationTimeout { get; init; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets how often workers report liveness and renew the leases on their in-flight modules.
    /// Defaults to 5 seconds.
    /// </summary>
    public TimeSpan WorkerHeartbeatInterval { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets how long a worker registration and its module leases remain valid without a
    /// heartbeat. Expired leases are requeued until <see cref="MaxModuleLeaseExpirations"/> is reached. Must exceed
    /// <see cref="WorkerHeartbeatInterval"/>. Defaults to 30 seconds.
    /// </summary>
    public TimeSpan WorkerTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets the number of lease expirations that fail a module permanently. Must be at least 1.
    /// Defaults to 2, allowing one recovery after a worker is lost. This budget is per module
    /// across all workers and is independent of the module's retry policy.
    /// </summary>
    public int MaxModuleLeaseExpirations { get; init; } = 2;

    /// <summary>
    /// Gets how long workers keep running without a sign of life from a master that has not
    /// signalled completion, or without being able to check on the master at all. After this period
    /// the worker cancels its in-flight modules and fails, so a master that exited, failed or crashed
    /// does not leave workers running. Must exceed <see cref="WorkerHeartbeatInterval"/>. Defaults to
    /// 1 minute.
    /// </summary>
    /// <remarks>
    /// Backends that hold a connection to the master, such as SignalR, detect a lost master when
    /// that connection closes for good and do not wait for this period.
    /// </remarks>
    public TimeSpan MasterTimeout { get; init; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Gets the minimum number of external workers that must register before
    /// the master starts dispatching work. The default is zero, which starts dispatching immediately.
    /// Waiting stops when <see cref="WorkerRegistrationTimeout"/> expires, after which dispatch proceeds
    /// with the workers currently available even when the configured minimum was not reached.
    /// </summary>
    public int MinimumWorkerCount { get; init; }

    /// <summary>
    /// Gets how long the master waits for a claimed module's result beyond the module's own
    /// timeouts. The worker enforces the module's per-attempt timeout; the master's deadline starts
    /// when a worker claims the module and allows every configured attempt plus this period.
    /// Defaults to 45 minutes. Set to <see cref="Timeout.InfiniteTimeSpan"/> to wait indefinitely.
    /// Zero and other negative durations are invalid.
    /// </summary>
    public TimeSpan ModuleResultTimeout { get; init; } = TimeSpan.FromMinutes(45);

    /// <summary>
    /// Gets which pipeline outcomes make <see cref="IDistributedRunCleanup"/> backends delete the data they
    /// stored for this run. Only the master cleans up, and only in distributed mode. Defaults to
    /// <see cref="DistributedRunCleanupPolicy.Always"/>; use <see cref="DistributedRunCleanupPolicy.OnSuccess"/>
    /// to keep a failed run's data for investigation.
    /// </summary>
    public DistributedRunCleanupPolicy RunCleanup { get; init; } = DistributedRunCleanupPolicy.Always;

    /// <summary>
    /// Gets how long run cleanup waits for workers to publish their final status, and how long each
    /// <see cref="IDistributedRunCleanup"/> backend may take to delete a run's data, before the pipeline stops
    /// waiting. Cleanup is skipped when a worker does not finish in time. Defaults to 2 minutes.
    /// </summary>
    public TimeSpan RunCleanupTimeout { get; init; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets the identifier this process uses when it registers, claims work and publishes results.
    /// </summary>
    internal WorkerId LocalWorkerId => WorkerId.FromInstanceIndex(InstanceIndex);
}
