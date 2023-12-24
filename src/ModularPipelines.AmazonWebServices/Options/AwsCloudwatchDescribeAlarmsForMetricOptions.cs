using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "describe-alarms-for-metric")]
public record AwsCloudwatchDescribeAlarmsForMetricOptions(
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
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

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}