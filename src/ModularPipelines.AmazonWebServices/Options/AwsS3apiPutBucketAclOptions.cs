using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "put-bucket-acl")]
public record AwsS3apiPutBucketAclOptions(
[property: CommandSwitch("--bucket")] string Bucket
) : AwsOptions
{
    [CommandSwitch("--acl")]
    public string? Acl { get; set; }

    [CommandSwitch("--access-control-policy")]
    public string? AccessControlPolicy { get; set; }

    [CommandSwitch("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CommandSwitch("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CommandSwitch("--grant-full-control")]
    public string? GrantFullControl { get; set; }

    [CommandSwitch("--grant-read")]
    public string? GrantRead { get; set; }

    [CommandSwitch("--grant-read-acp")]
    public string? GrantReadAcp { get; set; }

    [CommandSwitch("--grant-write")]
    public string? GrantWrite { get; set; }

    [CommandSwitch("--grant-write-acp")]
    public string? GrantWriteAcp { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}