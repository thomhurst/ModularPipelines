using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "create-log-anomaly-detector")]
public record AwsLogsCreateLogAnomalyDetectorOptions(
[property: CliOption("--log-group-arn-list")] string[] LogGroupArnList
) : AwsOptions
{
    [CliOption("--detector-name")]
    public string? DetectorName { get; set; }

    [CliOption("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [CliOption("--filter-pattern")]
    public string? FilterPattern { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--anomaly-visibility-time")]
    public long? AnomalyVisibilityTime { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}