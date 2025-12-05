using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "create-anomaly-detector")]
public record AwsLookoutmetricsCreateAnomalyDetectorOptions(
[property: CliOption("--anomaly-detector-name")] string AnomalyDetectorName,
[property: CliOption("--anomaly-detector-config")] string AnomalyDetectorConfig
) : AwsOptions
{
    [CliOption("--anomaly-detector-description")]
    public string? AnomalyDetectorDescription { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}