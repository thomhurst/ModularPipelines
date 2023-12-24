using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "complete-multipart-upload")]
public record AwsS3apiCompleteMultipartUploadOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--upload-id")] string UploadId
) : AwsOptions
{
    [CommandSwitch("--multipart-upload")]
    public string? MultipartUpload { get; set; }

    [CommandSwitch("--checksum-crc32")]
    public string? ChecksumCrc32 { get; set; }

    [CommandSwitch("--checksum-crc32-c")]
    public string? ChecksumCrc32C { get; set; }

    [CommandSwitch("--checksum-sha1")]
    public string? ChecksumSha1 { get; set; }

    [CommandSwitch("--checksum-sha256")]
    public string? ChecksumSha256 { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}