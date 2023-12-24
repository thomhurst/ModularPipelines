using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "update-anomaly")]
public record AwsLogsUpdateAnomalyOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CommandSwitch("--anomaly-id")]
    public string? AnomalyId { get; set; }

    [CommandSwitch("--pattern-id")]
    public string? PatternId { get; set; }

    [CommandSwitch("--suppression-type")]
    public string? SuppressionType { get; set; }

    [CommandSwitch("--suppression-period")]
    public string? SuppressionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}