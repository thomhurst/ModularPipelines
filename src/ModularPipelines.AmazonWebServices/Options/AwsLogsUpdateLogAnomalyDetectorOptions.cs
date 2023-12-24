using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "update-log-anomaly-detector")]
public record AwsLogsUpdateLogAnomalyDetectorOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CommandSwitch("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [CommandSwitch("--filter-pattern")]
    public string? FilterPattern { get; set; }

    [CommandSwitch("--anomaly-visibility-time")]
    public long? AnomalyVisibilityTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}