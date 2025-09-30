using ModularPipelines.Distributed.Abstractions;

namespace ModularPipelines.Distributed.Options;

/// <summary>
/// Configuration options for distributed pipeline execution.
/// </summary>
public sealed class DistributedPipelineOptions
{
    /// <summary>
    /// Gets or sets the execution mode for this node.
    /// </summary>
    public DistributedExecutionMode Mode { get; set; } = DistributedExecutionMode.Standalone;

    /// <summary>
    /// Gets or sets the orchestrator endpoint URL (used by workers).
    /// </summary>
    public string? OrchestratorUrl { get; set; }

    /// <summary>
    /// Gets or sets the port for the orchestrator to listen on.
    /// </summary>
    public int OrchestratorPort { get; set; } = 8080;

    /// <summary>
    /// Gets or sets the worker ID (used in Worker mode). If not specified, a unique ID will be generated.
    /// </summary>
    public string? WorkerId { get; set; }

    /// <summary>
    /// Gets or sets the port for the worker to listen on (used in Worker mode).
    /// </summary>
    public int WorkerPort { get; set; } = 9000;

    /// <summary>
    /// Gets or sets the worker capabilities (used in Worker mode).
    /// </summary>
    public WorkerCapabilities? WorkerCapabilities { get; set; }

    /// <summary>
    /// Gets or sets the timeout for worker heartbeats.
    /// </summary>
    public TimeSpan WorkerHeartbeatTimeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets or sets the interval for worker heartbeats.
    /// </summary>
    public TimeSpan WorkerHeartbeatInterval { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the timeout for remote execution requests.
    /// </summary>
    public TimeSpan RemoteExecutionTimeout { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Gets or sets the maximum number of retry attempts for failed executions.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets a value indicating whether to enable compression for network communication.
    /// </summary>
    public bool EnableCompression { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to prefer local execution when possible.
    /// </summary>
    public bool PreferLocalExecution { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum payload size in bytes (for validation).
    /// </summary>
    public long MaxPayloadSize { get; set; } = 100 * 1024 * 1024; // 100 MB

    /// <summary>
    /// Gets or sets the node registry implementation type.
    /// </summary>
    public Type? NodeRegistryType { get; set; }

    /// <summary>
    /// Gets or sets the remote communicator implementation type.
    /// </summary>
    public Type? RemoteCommunicatorType { get; set; }

    /// <summary>
    /// Gets or sets the result cache implementation type.
    /// </summary>
    public Type? ResultCacheType { get; set; }

    /// <summary>
    /// Gets or sets the distributed scheduler implementation type.
    /// </summary>
    public Type? DistributedSchedulerType { get; set; }
}

/// <summary>
/// Represents the execution mode for a distributed pipeline node.
/// </summary>
public enum DistributedExecutionMode
{
    /// <summary>
    /// Standalone mode - no distributed execution (default).
    /// </summary>
    Standalone,

    /// <summary>
    /// Orchestrator mode - coordinates module execution across workers.
    /// </summary>
    Orchestrator,

    /// <summary>
    /// Worker mode - executes modules assigned by the orchestrator.
    /// </summary>
    Worker,
}
