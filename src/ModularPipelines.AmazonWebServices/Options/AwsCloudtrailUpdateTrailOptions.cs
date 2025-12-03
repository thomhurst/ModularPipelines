using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "update-trail")]
public record AwsCloudtrailUpdateTrailOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--s3-bucket-name")]
    public string? S3BucketName { get; set; }

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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}