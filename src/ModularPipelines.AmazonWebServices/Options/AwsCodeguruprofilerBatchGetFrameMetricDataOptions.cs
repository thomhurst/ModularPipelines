using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "batch-get-frame-metric-data")]
public record AwsCodeguruprofilerBatchGetFrameMetricDataOptions(
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--frame-metrics")]
    public string[]? FrameMetrics { get; set; }

    [CommandSwitch("--period")]
    public string? Period { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--target-resolution")]
    public string? TargetResolution { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}