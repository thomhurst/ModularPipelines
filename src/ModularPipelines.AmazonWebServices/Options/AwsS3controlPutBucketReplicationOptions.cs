using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-bucket-replication")]
public record AwsS3controlPutBucketReplicationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--replication-configuration")] string ReplicationConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}