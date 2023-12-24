using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "update-anomaly-detector")]
public record AwsLookoutmetricsUpdateAnomalyDetectorOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--anomaly-detector-description")]
    public string? AnomalyDetectorDescription { get; set; }

    [CommandSwitch("--anomaly-detector-config")]
    public string? AnomalyDetectorConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}