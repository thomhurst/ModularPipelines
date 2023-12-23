using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "create-log-anomaly-detector")]
public record AwsLogsCreateLogAnomalyDetectorOptions(
[property: CommandSwitch("--log-group-arn-list")] string[] LogGroupArnList
) : AwsOptions
{
    [CommandSwitch("--detector-name")]
    public string? DetectorName { get; set; }

    [CommandSwitch("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [CommandSwitch("--filter-pattern")]
    public string? FilterPattern { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--anomaly-visibility-time")]
    public long? AnomalyVisibilityTime { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}