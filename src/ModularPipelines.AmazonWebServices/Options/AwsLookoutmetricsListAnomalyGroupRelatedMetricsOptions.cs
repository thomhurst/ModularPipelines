using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "list-anomaly-group-related-metrics")]
public record AwsLookoutmetricsListAnomalyGroupRelatedMetricsOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CliOption("--anomaly-group-id")] string AnomalyGroupId
) : AwsOptions
{
    [CliOption("--relationship-type-filter")]
    public string? RelationshipTypeFilter { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}