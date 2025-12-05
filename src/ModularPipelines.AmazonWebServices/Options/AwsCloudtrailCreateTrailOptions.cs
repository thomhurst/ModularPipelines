using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "create-trail")]
public record AwsCloudtrailCreateTrailOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
    [CliOption("--s3-key-prefix")]
    public string? S3KeyPrefix { get; set; }

    [CliOption("--sns-topic-name")]
    public string? SnsTopicName { get; set; }

    [CliOption("--cloud-watch-logs-log-group-arn")]
    public string? CloudWatchLogsLogGroupArn { get; set; }

    [CliOption("--cloud-watch-logs-role-arn")]
    public string? CloudWatchLogsRoleArn { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags-list")]
    public string[]? TagsList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}