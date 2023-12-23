using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "list-anomaly-group-related-metrics")]
public record AwsLookoutmetricsListAnomalyGroupRelatedMetricsOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CommandSwitch("--anomaly-group-id")] string AnomalyGroupId
) : AwsOptions
{
    [CommandSwitch("--relationship-type-filter")]
    public string? RelationshipTypeFilter { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}