using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "detect-metric-set-config")]
public record AwsLookoutmetricsDetectMetricSetConfigOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CommandSwitch("--auto-detection-metric-source")] string AutoDetectionMetricSource
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}