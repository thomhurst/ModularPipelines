using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "create-anomaly-detector")]
public record AwsLookoutmetricsCreateAnomalyDetectorOptions(
[property: CommandSwitch("--anomaly-detector-name")] string AnomalyDetectorName,
[property: CommandSwitch("--anomaly-detector-config")] string AnomalyDetectorConfig
) : AwsOptions
{
    [CommandSwitch("--anomaly-detector-description")]
    public string? AnomalyDetectorDescription { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}