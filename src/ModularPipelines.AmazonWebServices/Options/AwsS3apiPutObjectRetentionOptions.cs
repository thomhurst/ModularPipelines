using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-object-retention")]
public record AwsS3apiPutObjectRetentionOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--retention")]
    public string? Retention { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliOption("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}