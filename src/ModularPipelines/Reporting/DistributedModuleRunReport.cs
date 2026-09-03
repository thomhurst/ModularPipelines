namespace ModularPipelines.Reporting;

/// <summary>
/// Describes distributed timing for one module.
/// </summary>
public sealed record DistributedModuleRunReport
{
    /// <summary>Gets the assembly-qualified module type name.</summary>
    public string ModuleTypeName { get; init; } = string.Empty;

    /// <summary>Gets the index of the worker that executed the module.</summary>
    public int WorkerIndex { get; init; }

    /// <summary>Gets when the assignment was enqueued.</summary>
    public DateTimeOffset EnqueuedAt { get; init; }

    /// <summary>Gets when the worker claimed the assignment.</summary>
    public DateTimeOffset ClaimedAt { get; init; }

    /// <summary>Gets when module execution started.</summary>
    public DateTimeOffset ExecutionStartedAt { get; init; }

    /// <summary>Gets when module execution finished.</summary>
    public DateTimeOffset ExecutionFinishedAt { get; init; }

    /// <summary>Gets when the serialized result was ready to publish.</summary>
    public DateTimeOffset ResultReadyAt { get; init; }

    /// <summary>Gets the time between enqueue and claim.</summary>
    public TimeSpan QueueWaitDuration { get; init; }

    /// <summary>Gets the module execution duration.</summary>
    public TimeSpan ExecutionDuration { get; init; }

    /// <summary>Gets the time spent publishing the assignment.</summary>
    public TimeSpan AssignmentPublishDuration { get; init; }

    /// <summary>Gets the measured dependency-result transfer duration.</summary>
    public TimeSpan DependencyResultTransferDuration { get; init; }

    /// <summary>Gets the time spent applying dependency results on the worker.</summary>
    public TimeSpan DependencyResultProcessingDuration { get; init; }

    /// <summary>Gets the time spent downloading consumed artifacts.</summary>
    public TimeSpan ArtifactDownloadDuration { get; init; }

    /// <summary>Gets the time spent uploading produced artifacts.</summary>
    public TimeSpan ArtifactUploadDuration { get; init; }

    /// <summary>Gets the time between result serialization and collection.</summary>
    public TimeSpan ResultTransferDuration { get; init; }

    /// <summary>Gets the sum of measured distributed overhead.</summary>
    public TimeSpan TotalOverheadDuration { get; init; }
}
