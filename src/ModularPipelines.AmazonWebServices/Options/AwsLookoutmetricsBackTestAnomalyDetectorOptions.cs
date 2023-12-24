using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "back-test-anomaly-detector")]
public record AwsLookoutmetricsBackTestAnomalyDetectorOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}