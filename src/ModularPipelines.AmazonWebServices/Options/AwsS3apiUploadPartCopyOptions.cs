using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "upload-part-copy")]
public record AwsS3apiUploadPartCopyOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--copy-source")] string CopySource,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--part-number")] int PartNumber,
[property: CommandSwitch("--upload-id")] string UploadId
) : AwsOptions
{
    [CommandSwitch("--copy-source-if-match")]
    public string? CopySourceIfMatch { get; set; }

    [CommandSwitch("--copy-source-if-modified-since")]
    public long? CopySourceIfModifiedSince { get; set; }

    [CommandSwitch("--copy-source-if-none-match")]
    public string? CopySourceIfNoneMatch { get; set; }

    [CommandSwitch("--copy-source-if-unmodified-since")]
    public long? CopySourceIfUnmodifiedSince { get; set; }

    [CommandSwitch("--copy-source-range")]
    public string? CopySourceRange { get; set; }

    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--copy-source-sse-customer-algorithm")]
    public string? CopySourceSseCustomerAlgorithm { get; set; }

    [CommandSwitch("--copy-source-sse-customer-key")]
    public string? CopySourceSseCustomerKey { get; set; }

    [CommandSwitch("--copy-source-sse-customer-key-md5")]
    public string? CopySourceSseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--expected-source-bucket-owner")]
    public string? ExpectedSourceBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}