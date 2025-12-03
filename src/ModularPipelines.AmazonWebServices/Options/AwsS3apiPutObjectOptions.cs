using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "put-object")]
public record AwsS3apiPutObjectOptions(
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--key")] string Key
) : AwsOptions
{
    [CliOption("--acl")]
    public string? Acl { get; set; }

    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--cache-control")]
    public string? CacheControl { get; set; }

    [CliOption("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CliOption("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CliOption("--content-language")]
    public string? ContentLanguage { get; set; }

    [CliOption("--content-length")]
    public long? ContentLength { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

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

    [CliOption("--expires")]
    public long? Expires { get; set; }

    [CliOption("--grant-full-control")]
    public string? GrantFullControl { get; set; }

    [CliOption("--grant-read")]
    public string? GrantRead { get; set; }

    [CliOption("--grant-read-acp")]
    public string? GrantReadAcp { get; set; }

    [CliOption("--grant-write-acp")]
    public string? GrantWriteAcp { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--server-side-encryption")]
    public string? ServerSideEncryption { get; set; }

    [CliOption("--storage-class")]
    public string? StorageClass { get; set; }

    [CliOption("--website-redirect-location")]
    public string? WebsiteRedirectLocation { get; set; }

    [CliOption("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CliOption("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CliOption("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CliOption("--ssekms-key-id")]
    public string? SsekmsKeyId { get; set; }

    [CliOption("--ssekms-encryption-context")]
    public string? SsekmsEncryptionContext { get; set; }

    [CliOption("--request-payer")]
    public string? RequestPayer { get; set; }

    [CliOption("--tagging")]
    public string? Tagging { get; set; }

    [CliOption("--object-lock-mode")]
    public string? ObjectLockMode { get; set; }

    [CliOption("--object-lock-retain-until-date")]
    public long? ObjectLockRetainUntilDate { get; set; }

    [CliOption("--object-lock-legal-hold-status")]
    public string? ObjectLockLegalHoldStatus { get; set; }

    [CliOption("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}