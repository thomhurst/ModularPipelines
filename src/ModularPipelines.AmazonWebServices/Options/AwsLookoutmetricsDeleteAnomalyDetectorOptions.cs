using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "delete-anomaly-detector")]
public record AwsLookoutmetricsDeleteAnomalyDetectorOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}