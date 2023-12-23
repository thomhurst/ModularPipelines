using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "put-metric-alarm")]
public record AwsCloudwatchPutMetricAlarmOptions(
[property: CommandSwitch("--alarm-name")] string AlarmName,
[property: CommandSwitch("--evaluation-periods")] int EvaluationPeriods,
[property: CommandSwitch("--comparison-operator")] string ComparisonOperator
) : AwsOptions
{
    [CommandSwitch("--alarm-description")]
    public string? AlarmDescription { get; set; }

    [CommandSwitch("--ok-actions")]
    public string[]? OkActions { get; set; }

    [CommandSwitch("--alarm-actions")]
    public string[]? AlarmActions { get; set; }

    [CommandSwitch("--insufficient-data-actions")]
    public string[]? InsufficientDataActions { get; set; }

    [CommandSwitch("--metric-name")]
    public string? MetricName { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--statistic")]
    public string? Statistic { get; set; }

    [CommandSwitch("--extended-statistic")]
    public string? ExtendedStatistic { get; set; }

    [CommandSwitch("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CommandSwitch("--period")]
    public int? Period { get; set; }

    [CommandSwitch("--unit")]
    public string? Unit { get; set; }

    [CommandSwitch("--datapoints-to-alarm")]
    public int? DatapointsToAlarm { get; set; }

    [CommandSwitch("--threshold")]
    public double? Threshold { get; set; }

    [CommandSwitch("--treat-missing-data")]
    public string? TreatMissingData { get; set; }

    [CommandSwitch("--evaluate-low-sample-count-percentile")]
    public string? EvaluateLowSampleCountPercentile { get; set; }

    [CommandSwitch("--metrics")]
    public string[]? Metrics { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--threshold-metric-id")]
    public string? ThresholdMetricId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}