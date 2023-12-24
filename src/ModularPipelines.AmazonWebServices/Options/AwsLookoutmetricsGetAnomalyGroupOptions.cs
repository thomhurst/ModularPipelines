using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "get-anomaly-group")]
public record AwsLookoutmetricsGetAnomalyGroupOptions(
[property: CommandSwitch("--anomaly-group-id")] string AnomalyGroupId,
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}