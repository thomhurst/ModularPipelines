using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-bucket-replication")]
public record AwsS3controlPutBucketReplicationOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--replication-configuration")] string ReplicationConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}