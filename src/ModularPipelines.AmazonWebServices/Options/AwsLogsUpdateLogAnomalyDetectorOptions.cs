using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "update-log-anomaly-detector")]
public record AwsLogsUpdateLogAnomalyDetectorOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn
) : AwsOptions
{
    [CliOption("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [CliOption("--filter-pattern")]
    public string? FilterPattern { get; set; }

    [CliOption("--anomaly-visibility-time")]
    public long? AnomalyVisibilityTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}