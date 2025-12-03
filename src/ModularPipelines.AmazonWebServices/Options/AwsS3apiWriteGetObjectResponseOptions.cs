using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3api", "write-get-object-response")]
public record AwsS3apiWriteGetObjectResponseOptions(
[property: CliOption("--request-route")] string RequestRoute,
[property: CliOption("--request-token")] string RequestToken
) : AwsOptions
{
    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--status-code")]
    public int? StatusCode { get; set; }

    [CliOption("--error-code")]
    public string? ErrorCode { get; set; }

    [CliOption("--error-message")]
    public string? ErrorMessage { get; set; }

    [CliOption("--accept-ranges")]
    public string? AcceptRanges { get; set; }

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

    [CliOption("--content-range")]
    public string? ContentRange { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--checksum-crc32")]
    public string? ChecksumCrc32 { get; set; }

    [CliOption("--checksum-crc32-c")]
    public string? ChecksumCrc32C { get; set; }

    [CliOption("--checksum-sha1")]
    public string? ChecksumSha1 { get; set; }

    [CliOption("--checksum-sha256")]
    public string? ChecksumSha256 { get; set; }

    [CliOption("--e-tag")]
    public string? ETag { get; set; }

    [CliOption("--expires")]
    public long? Expires { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--last-modified")]
    public long? LastModified { get; set; }

    [CliOption("--missing-meta")]
    public int? MissingMeta { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--object-lock-mode")]
    public string? ObjectLockMode { get; set; }

    [CliOption("--object-lock-legal-hold-status")]
    public string? ObjectLockLegalHoldStatus { get; set; }

    [CliOption("--object-lock-retain-until-date")]
    public long? ObjectLockRetainUntilDate { get; set; }

    [CliOption("--parts-count")]
    public int? PartsCount { get; set; }

    [CliOption("--replication-status")]
    public string? ReplicationStatus { get; set; }

    [CliOption("--request-charged")]
    public string? RequestCharged { get; set; }

    [CliOption("--restore")]
    public string? Restore { get; set; }

    [CliOption("--server-side-encryption")]
    public string? ServerSideEncryption { get; set; }

    [CliOption("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CliOption("--ssekms-key-id")]
    public string? SsekmsKeyId { get; set; }

    [CliOption("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CliOption("--storage-class")]
    public string? StorageClass { get; set; }

    [CliOption("--tag-count")]
    public int? TagCount { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}