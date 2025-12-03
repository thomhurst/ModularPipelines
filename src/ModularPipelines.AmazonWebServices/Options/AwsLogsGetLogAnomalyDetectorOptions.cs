using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "get-log-anomaly-detector")]
public record AwsLogsGetLogAnomalyDetectorOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}