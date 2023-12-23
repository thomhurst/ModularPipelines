using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "get-metric-statistics")]
public record AwsCloudwatchGetMetricStatisticsOptions(
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--period")] int Period
) : AwsOptions
{
    [CommandSwitch("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CommandSwitch("--statistics")]
    public string[]? Statistics { get; set; }

    [CommandSwitch("--extended-statistics")]
    public string[]? ExtendedStatistics { get; set; }

    [CommandSwitch("--unit")]
    public string? Unit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}