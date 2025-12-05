using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "list-anomaly-group-summaries")]
public record AwsLookoutmetricsListAnomalyGroupSummariesOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CliOption("--sensitivity-threshold")] int SensitivityThreshold
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}