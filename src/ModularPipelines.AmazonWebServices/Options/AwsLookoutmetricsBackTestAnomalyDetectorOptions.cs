using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "back-test-anomaly-detector")]
public record AwsLookoutmetricsBackTestAnomalyDetectorOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}