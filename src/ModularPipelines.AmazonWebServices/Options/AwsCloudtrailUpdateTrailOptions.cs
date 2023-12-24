using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "update-trail")]
public record AwsCloudtrailUpdateTrailOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--s3-bucket-name")]
    public string? S3BucketName { get; set; }

    [CommandSwitch("--s3-key-prefix")]
    public string? S3KeyPrefix { get; set; }

    [CommandSwitch("--sns-topic-name")]
    public string? SnsTopicName { get; set; }

    [CommandSwitch("--cloud-watch-logs-log-group-arn")]
    public string? CloudWatchLogsLogGroupArn { get; set; }

    [CommandSwitch("--cloud-watch-logs-role-arn")]
    public string? CloudWatchLogsRoleArn { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}