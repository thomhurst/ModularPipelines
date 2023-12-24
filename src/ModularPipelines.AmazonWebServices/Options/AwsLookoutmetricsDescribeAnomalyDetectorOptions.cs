using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "describe-anomaly-detector")]
public record AwsLookoutmetricsDescribeAnomalyDetectorOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}