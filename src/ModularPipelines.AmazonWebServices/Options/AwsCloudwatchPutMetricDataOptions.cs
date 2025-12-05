using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-metric-data")]
public record AwsCloudwatchPutMetricDataOptions(
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--metric-data")]
    public string[]? MetricData { get; set; }

    [CliOption("--metric-name")]
    public string? MetricName { get; set; }

    [CliOption("--timestamp")]
    public string? Timestamp { get; set; }

    [CliOption("--unit")]
    public string? Unit { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }

    [CliOption("--dimensions")]
    public string? Dimensions { get; set; }

    [CliOption("--statistic-values")]
    public string? StatisticValues { get; set; }

    [CliOption("--storage-resolution")]
    public string? StorageResolution { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}