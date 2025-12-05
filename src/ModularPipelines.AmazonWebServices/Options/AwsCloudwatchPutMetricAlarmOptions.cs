using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-metric-alarm")]
public record AwsCloudwatchPutMetricAlarmOptions(
[property: CliOption("--alarm-name")] string AlarmName,
[property: CliOption("--evaluation-periods")] int EvaluationPeriods,
[property: CliOption("--comparison-operator")] string ComparisonOperator
) : AwsOptions
{
    [CliOption("--alarm-description")]
    public string? AlarmDescription { get; set; }

    [CliOption("--ok-actions")]
    public string[]? OkActions { get; set; }

    [CliOption("--alarm-actions")]
    public string[]? AlarmActions { get; set; }

    [CliOption("--insufficient-data-actions")]
    public string[]? InsufficientDataActions { get; set; }

    [CliOption("--metric-name")]
    public string? MetricName { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--statistic")]
    public string? Statistic { get; set; }

    [CliOption("--extended-statistic")]
    public string? ExtendedStatistic { get; set; }

    [CliOption("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CliOption("--period")]
    public int? Period { get; set; }

    [CliOption("--unit")]
    public string? Unit { get; set; }

    [CliOption("--datapoints-to-alarm")]
    public int? DatapointsToAlarm { get; set; }

    [CliOption("--threshold")]
    public double? Threshold { get; set; }

    [CliOption("--treat-missing-data")]
    public string? TreatMissingData { get; set; }

    [CliOption("--evaluate-low-sample-count-percentile")]
    public string? EvaluateLowSampleCountPercentile { get; set; }

    [CliOption("--metrics")]
    public string[]? Metrics { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--threshold-metric-id")]
    public string? ThresholdMetricId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}