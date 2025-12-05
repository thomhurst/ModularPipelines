using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "get-object")]
public record AwsS3apiGetObjectOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public long? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public long? IfUnmodifiedSince { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }

    [CliOption("--response-cache-control")]
    public string? ResponseCacheControl { get; set; }

    [CliOption("--response-content-disposition")]
    public string? ResponseContentDisposition { get; set; }

    [CliOption("--response-content-encoding")]
    public string? ResponseContentEncoding { get; set; }

    [CliOption("--response-content-language")]
    public string? ResponseContentLanguage { get; set; }

    [CliOption("--response-content-type")]
    public string? ResponseContentType { get; set; }

    [CliOption("--response-expires")]
    public long? ResponseExpires { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CliOption("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CliOption("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--part-number")]
    public int? PartNumber { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--checksum-mode")]
    public string? ChecksumMode { get; set; }
}