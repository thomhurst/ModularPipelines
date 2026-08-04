using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Describes one module in a machine-readable pipeline run report.
/// </summary>
public sealed record ModuleRunReport
{
    /// <summary>
    /// Gets the module name.
    /// </summary>
    public string ModuleName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the stable assembly-qualified module type identifier.
    /// </summary>
    public string ModuleTypeName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the final module status.
    /// </summary>
    public Status Status { get; init; }

    /// <summary>
    /// Gets the module duration.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Gets a value indicating whether the duration was measured during this run.
    /// </summary>
    public bool DurationMeasured { get; init; }

    /// <summary>
    /// Gets when the module started.
    /// </summary>
    public DateTimeOffset? Start { get; init; }

    /// <summary>
    /// Gets when the module finished.
    /// </summary>
    public DateTimeOffset? End { get; init; }

    /// <summary>
    /// Gets the skip reason, when the module was skipped.
    /// </summary>
    public string? SkipReason { get; init; }

    /// <summary>
    /// Gets exception details, when the module failed.
    /// </summary>
    public RunReportExceptionDetails? Exception { get; init; }

    /// <summary>
    /// Gets the optional size-capped, secret-masked module output excerpt.
    /// </summary>
    public ModuleOutputExcerpt? Output { get; init; }

    /// <summary>
    /// Gets the number of commands attempted by the module.
    /// </summary>
    public int CommandCount { get; init; }

    /// <summary>
    /// Gets this module's duration in the previous retained run, when available.
    /// </summary>
    public TimeSpan? PreviousDuration { get; init; }

    /// <summary>
    /// Gets the duration change from the previous retained run, when available.
    /// </summary>
    public TimeSpan? DurationDelta { get; init; }
}
