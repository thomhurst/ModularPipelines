using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "send-command")]
public record AwsSsmSendCommandOptions(
[property: CommandSwitch("--document-name")] string DocumentName
) : AwsOptions
{
    [CommandSwitch("--instance-ids")]
    public string[]? InstanceIds { get; set; }

    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

    [CommandSwitch("--document-version")]
    public string? DocumentVersion { get; set; }

    [CommandSwitch("--document-hash")]
    public string? DocumentHash { get; set; }

    [CommandSwitch("--document-hash-type")]
    public string? DocumentHashType { get; set; }

    [CommandSwitch("--timeout-seconds")]
    public int? TimeoutSeconds { get; set; }

    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--output-s3-region")]
    public string? OutputS3Region { get; set; }

    [CommandSwitch("--output-s3-bucket-name")]
    public string? OutputS3BucketName { get; set; }

    [CommandSwitch("--output-s3-key-prefix")]
    public string? OutputS3KeyPrefix { get; set; }

    [CommandSwitch("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CommandSwitch("--max-errors")]
    public string? MaxErrors { get; set; }

    [CommandSwitch("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

    [CommandSwitch("--notification-config")]
    public string? NotificationConfig { get; set; }

    [CommandSwitch("--cloud-watch-output-config")]
    public string? CloudWatchOutputConfig { get; set; }

    [CommandSwitch("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}