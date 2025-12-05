using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-object-acl")]
public record AwsS3apiPutObjectAclOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--acl")]
    public string? Acl { get; set; }

    [CliOption("--access-control-policy")]
    public string? AccessControlPolicy { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliOption("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CliOption("--grant-full-control")]
    public string? GrantFullControl { get; set; }

    [CliOption("--grant-read")]
    public string? GrantRead { get; set; }

    [CliOption("--grant-read-acp")]
    public string? GrantReadAcp { get; set; }

    [CliOption("--grant-write")]
    public string? GrantWrite { get; set; }

    [CliOption("--grant-write-acp")]
    public string? GrantWriteAcp { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}