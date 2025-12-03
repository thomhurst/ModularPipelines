using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "send-command")]
public record AwsSsmSendCommandOptions(
[property: CliOption("--document-name")] string DocumentName
) : AwsOptions
{
    [CliOption("--instance-ids")]
    public string[]? InstanceIds { get; set; }

    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--document-hash")]
    public string? DocumentHash { get; set; }

    [CliOption("--document-hash-type")]
    public string? DocumentHashType { get; set; }

    [CliOption("--timeout-seconds")]
    public int? TimeoutSeconds { get; set; }

    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--output-s3-region")]
    public string? OutputS3Region { get; set; }

    [CliOption("--output-s3-bucket-name")]
    public string? OutputS3BucketName { get; set; }

    [CliOption("--output-s3-key-prefix")]
    public string? OutputS3KeyPrefix { get; set; }

    [CliOption("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CliOption("--max-errors")]
    public string? MaxErrors { get; set; }

    [CliOption("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

    [CliOption("--notification-config")]
    public string? NotificationConfig { get; set; }

    [CliOption("--cloud-watch-output-config")]
    public string? CloudWatchOutputConfig { get; set; }

    [CliOption("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}