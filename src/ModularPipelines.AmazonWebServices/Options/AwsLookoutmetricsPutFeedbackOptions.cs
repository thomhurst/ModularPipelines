using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "put-feedback")]
public record AwsLookoutmetricsPutFeedbackOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CommandSwitch("--anomaly-group-time-series-feedback")] string AnomalyGroupTimeSeriesFeedback
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}