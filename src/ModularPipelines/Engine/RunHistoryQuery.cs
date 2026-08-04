using ModularPipelines.Enums;

namespace ModularPipelines.Engine;

/// <summary>
/// Describes a bounded query over retained pipeline run reports.
/// </summary>
public sealed record RunHistoryQuery
{
    /// <summary>
    /// Gets the stable pipeline identity whose history should be queried.
    /// </summary>
    public required string PipelineIdentity { get; init; }

    /// <summary>
    /// Gets the maximum number of reports to return.
    /// </summary>
    public int MaxRuns { get; init; } = 20;

    /// <summary>
    /// Gets the inclusive lower bound for a report's end time.
    /// </summary>
    public DateTimeOffset? Since { get; init; }

    /// <summary>
    /// Gets the pipeline status to match, or <see langword="null"/> for every status.
    /// </summary>
    public Status? Status { get; init; }
}
