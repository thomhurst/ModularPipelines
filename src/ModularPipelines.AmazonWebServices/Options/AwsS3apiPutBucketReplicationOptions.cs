using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "put-bucket-replication")]
public record AwsS3apiPutBucketReplicationOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--replication-configuration")] string ReplicationConfiguration
) : AwsOptions
{
    [CommandSwitch("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CommandSwitch("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}