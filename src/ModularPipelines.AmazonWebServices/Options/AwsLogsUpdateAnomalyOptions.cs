using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "update-anomaly")]
public record AwsLogsUpdateAnomalyOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CliOption("--anomaly-id")]
    public string? AnomalyId { get; set; }

    [CliOption("--pattern-id")]
    public string? PatternId { get; set; }

    [CliOption("--suppression-type")]
    public string? SuppressionType { get; set; }

    [CliOption("--suppression-period")]
    public string? SuppressionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}