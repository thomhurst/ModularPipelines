namespace ModularPipelines.Distributed;

/// <summary>
/// Describes why the master canceled distributed execution.
/// </summary>
public enum DistributedCancellationReason
{
    /// <summary>
    /// A module failed. Workers cancel non-AlwaysRun modules, stop claiming non-AlwaysRun work and
    /// keep executing AlwaysRun assignments.
    /// </summary>
    PipelineFailed,

    /// <summary>
    /// The pipeline was stopped by the user or host. Workers cancel all work and claim nothing further.
    /// </summary>
    Stopped,
}
