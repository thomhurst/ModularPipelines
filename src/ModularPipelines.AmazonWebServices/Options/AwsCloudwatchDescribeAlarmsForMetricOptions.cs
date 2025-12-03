using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "describe-alarms-for-metric")]
public record AwsCloudwatchDescribeAlarmsForMetricOptions(
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}