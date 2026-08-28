using System.Text.Json.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.JsonUtils;

namespace ModularPipelines.Models;

/// <summary>
/// Represents a schema-versioned, machine-readable pipeline run report.
/// </summary>
public sealed record PipelineRunReport
{
    /// <summary>
    /// Gets the current run report schema version.
    /// </summary>
    public const int CurrentSchemaVersion = 4;

    /// <summary>
    /// Gets the schema version used by this report.
    /// </summary>
    public int SchemaVersion { get; init; } = CurrentSchemaVersion;

    /// <summary>
    /// Gets the unique identifier assigned to this pipeline run.
    /// </summary>
    public string RunId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the stable identity used to partition this pipeline's retained history.
    /// </summary>
    public string? PipelineIdentity { get; init; }

    /// <summary>
    /// Gets source-control, machine, and CI correlation metadata.
    /// </summary>
    public RunCorrelation? Correlation { get; init; }

    /// <summary>
    /// Gets the pipeline's final status.
    /// </summary>
    [JsonConverter(typeof(RunHistoryModuleStatusJsonConverter))]
    public ModuleStatus Status { get; init; }

    /// <summary>
    /// Gets when the pipeline started.
    /// </summary>
    public DateTimeOffset Start { get; init; }

    /// <summary>
    /// Gets when the pipeline finished.
    /// </summary>
    public DateTimeOffset End { get; init; }

    /// <summary>
    /// Gets the pipeline wall-clock duration.
    /// </summary>
    public TimeSpan TotalDuration { get; init; }

    /// <summary>
    /// Gets when the previous retained run used as the delta baseline finished.
    /// </summary>
    public DateTimeOffset? PreviousEnd { get; init; }

    /// <summary>
    /// Gets the previous retained run's total duration, when available.
    /// </summary>
    public TimeSpan? PreviousTotalDuration { get; init; }

    /// <summary>
    /// Gets the total duration change from the previous retained run, when available.
    /// </summary>
    public TimeSpan? TotalDurationDelta { get; init; }

    /// <summary>
    /// Gets execution metrics, when available.
    /// </summary>
    public PipelineMetrics? Metrics { get; init; }

    /// <summary>
    /// Gets the pipeline-level exception, when the run failed outside a module.
    /// </summary>
    public RunReportExceptionDetails? Exception { get; init; }

    /// <summary>
    /// Gets module run details.
    /// </summary>
    public IReadOnlyList<ModuleRunReport> Modules { get; init; } = [];

    /// <summary>
    /// Gets the total number of commands attempted during the run.
    /// </summary>
    public int CommandCount { get; init; }

    /// <summary>
    /// Gets the number of commands attempted outside a module execution context.
    /// </summary>
    public int UnattributedCommandCount { get; init; }
}
