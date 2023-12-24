using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "get-object")]
public record AwsS3apiGetObjectOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--key")] string Key
) : AwsOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-modified-since")]
    public long? IfModifiedSince { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public long? IfUnmodifiedSince { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }

    [CommandSwitch("--response-cache-control")]
    public string? ResponseCacheControl { get; set; }

    [CommandSwitch("--response-content-disposition")]
    public string? ResponseContentDisposition { get; set; }

    [CommandSwitch("--response-content-encoding")]
    public string? ResponseContentEncoding { get; set; }

    [CommandSwitch("--response-content-language")]
    public string? ResponseContentLanguage { get; set; }

    [CommandSwitch("--response-content-type")]
    public string? ResponseContentType { get; set; }

    [CommandSwitch("--response-expires")]
    public long? ResponseExpires { get; set; }

    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--part-number")]
    public int? PartNumber { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--checksum-mode")]
    public string? ChecksumMode { get; set; }
}