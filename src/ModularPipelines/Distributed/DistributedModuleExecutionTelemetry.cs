namespace ModularPipelines.Distributed;

/// <summary>
/// Describes worker-side timing for one distributed module execution.
/// </summary>
public sealed record DistributedModuleExecutionTelemetry
{
    /// <summary>
    /// Gets when the worker claimed the assignment.
    /// </summary>
    public DateTimeOffset ClaimedAt { get; init; }

    /// <summary>
    /// Gets when module execution started.
    /// </summary>
    public DateTimeOffset ExecutionStartedAt { get; init; }

    /// <summary>
    /// Gets when module execution finished.
    /// </summary>
    public DateTimeOffset ExecutionFinishedAt { get; init; }

    /// <summary>
    /// Gets time spent applying transferred dependency results.
    /// </summary>
    public TimeSpan DependencyResultProcessingDuration { get; init; }

    /// <summary>
    /// Gets time spent downloading consumed artifacts.
    /// </summary>
    public TimeSpan ArtifactDownloadDuration { get; init; }

    /// <summary>
    /// Gets time spent uploading produced artifacts.
    /// </summary>
    public TimeSpan ArtifactUploadDuration { get; init; }
}
