using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "list-anomaly-group-time-series")]
public record AwsLookoutmetricsListAnomalyGroupTimeSeriesOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CliOption("--anomaly-group-id")] string AnomalyGroupId,
[property: CliOption("--metric-name")] string MetricName
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}