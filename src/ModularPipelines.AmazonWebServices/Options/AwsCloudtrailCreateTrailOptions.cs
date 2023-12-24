using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "create-trail")]
public record AwsCloudtrailCreateTrailOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
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

    [CommandSwitch("--tags-list")]
    public string[]? TagsList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}