using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "get-metric-statistics")]
public record AwsCloudwatchGetMetricStatisticsOptions(
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--period")] int Period
) : AwsOptions
{
    [CliOption("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CliOption("--statistics")]
    public string[]? Statistics { get; set; }

    [CliOption("--extended-statistics")]
    public string[]? ExtendedStatistics { get; set; }

    [CliOption("--unit")]
    public string? Unit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}