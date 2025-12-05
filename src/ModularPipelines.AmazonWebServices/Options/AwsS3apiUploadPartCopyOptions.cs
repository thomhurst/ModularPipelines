using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "upload-part-copy")]
public record AwsS3apiUploadPartCopyOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--copy-source")] string CopySource,
[property: CliOption("--key")] string Key,
[property: CliOption("--part-number")] int PartNumber,
[property: CliOption("--upload-id")] string UploadId
) : AwsOptions
{
    [CliOption("--copy-source-if-match")]
    public string? CopySourceIfMatch { get; set; }

    [CliOption("--copy-source-if-modified-since")]
    public long? CopySourceIfModifiedSince { get; set; }

    [CliOption("--copy-source-if-none-match")]
    public string? CopySourceIfNoneMatch { get; set; }

    [CliOption("--copy-source-if-unmodified-since")]
    public long? CopySourceIfUnmodifiedSince { get; set; }

    [CliOption("--copy-source-range")]
    public string? CopySourceRange { get; set; }

    [CliOption("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CliOption("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CliOption("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CliOption("--copy-source-sse-customer-algorithm")]
    public string? CopySourceSseCustomerAlgorithm { get; set; }

    [CliOption("--copy-source-sse-customer-key")]
    public string? CopySourceSseCustomerKey { get; set; }

    [CliOption("--copy-source-sse-customer-key-md5")]
    public string? CopySourceSseCustomerKeyMd5 { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--expected-source-bucket-owner")]
    public string? ExpectedSourceBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}