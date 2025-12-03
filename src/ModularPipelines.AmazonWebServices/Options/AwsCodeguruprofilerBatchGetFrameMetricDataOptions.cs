using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "batch-get-frame-metric-data")]
public record AwsCodeguruprofilerBatchGetFrameMetricDataOptions(
[property: CliOption("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--frame-metrics")]
    public string[]? FrameMetrics { get; set; }

    [CliOption("--period")]
    public string? Period { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--target-resolution")]
    public string? TargetResolution { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}