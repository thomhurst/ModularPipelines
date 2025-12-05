using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "upload-part")]
public record AwsS3apiUploadPartOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key,
[property: CliOption("--part-number")] int PartNumber,
[property: CliOption("--upload-id")] string UploadId
) : AwsOptions
{
    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--content-length")]
    public long? ContentLength { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliOption("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CliOption("--checksum-crc32")]
    public string? ChecksumCrc32 { get; set; }

    [CliOption("--checksum-crc32-c")]
    public string? ChecksumCrc32C { get; set; }

    [CliOption("--checksum-sha1")]
    public string? ChecksumSha1 { get; set; }

    [CliOption("--checksum-sha256")]
    public string? ChecksumSha256 { get; set; }

    [CliOption("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CliOption("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CliOption("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}