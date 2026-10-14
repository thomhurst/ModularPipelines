namespace ModularPipelines.Distributed;

/// <summary>
/// Describes a pipeline run whose stored data <see cref="IDistributedRunCleanup"/> deletes.
/// </summary>
public sealed record DistributedRunCleanupContext
{
    /// <summary>Gets the identifier shared by every process in the run.</summary>
    public required string RunId { get; init; }

    /// <summary>Gets whether the pipeline completed without failing.</summary>
    public required bool PipelineSucceeded { get; init; }
}
