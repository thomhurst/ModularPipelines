using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModularPipelines.Models;

/// <summary>
/// Contains metrics about pipeline execution performance and parallelism.
/// </summary>
[ExcludeFromCodeCoverage]
public record PipelineMetrics
{
    /// <summary>
    /// Gets the parallelism factor: total module execution time / wall-clock time.
    /// A value greater than 1 indicates effective parallelization.
    /// For example, 4.2 means the pipeline ran 4.2x faster than sequential execution.
    /// </summary>
    [JsonInclude]
    public double ParallelismFactor { get; init; }

    /// <summary>
    /// Gets the maximum number of modules executing concurrently at any point.
    /// </summary>
    [JsonInclude]
    public int PeakConcurrency { get; init; }

    /// <summary>
    /// Gets the average number of modules executing concurrently.
    /// </summary>
    [JsonInclude]
    public double AverageConcurrency { get; init; }

    /// <summary>
    /// Gets the sum of all individual module execution durations.
    /// This represents how long the pipeline would take if run sequentially.
    /// </summary>
    [JsonInclude]
    public TimeSpan TotalModuleExecutionTime { get; init; }

    /// <summary>
    /// Gets the actual wall-clock duration of the pipeline.
    /// </summary>
    [JsonInclude]
    public TimeSpan WallClockDuration { get; init; }

    /// <summary>
    /// Gets the efficiency percentage: actual parallelism / theoretical maximum parallelism.
    /// Values closer to 1.0 indicate better utilization of available parallelism.
    /// </summary>
    [JsonInclude]
    public double Efficiency { get; init; }

    /// <summary>
    /// Gets the total number of modules in the pipeline.
    /// </summary>
    [JsonInclude]
    public int TotalModules { get; init; }

    /// <summary>
    /// Gets the number of modules that completed successfully.
    /// </summary>
    [JsonInclude]
    public int SuccessfulModules { get; init; }

    /// <summary>
    /// Gets the number of modules that failed.
    /// </summary>
    [JsonInclude]
    public int FailedModules { get; init; }

    /// <summary>
    /// Gets the number of modules that were skipped.
    /// </summary>
    [JsonInclude]
    public int SkippedModules { get; init; }
}
