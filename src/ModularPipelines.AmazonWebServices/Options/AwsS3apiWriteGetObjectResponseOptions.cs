using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "write-get-object-response")]
public record AwsS3apiWriteGetObjectResponseOptions(
[property: CommandSwitch("--request-route")] string RequestRoute,
[property: CommandSwitch("--request-token")] string RequestToken
) : AwsOptions
{
    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--status-code")]
    public int? StatusCode { get; set; }

    [CommandSwitch("--error-code")]
    public string? ErrorCode { get; set; }

    [CommandSwitch("--error-message")]
    public string? ErrorMessage { get; set; }

    [CommandSwitch("--accept-ranges")]
    public string? AcceptRanges { get; set; }

    [CommandSwitch("--cache-control")]
    public string? CacheControl { get; set; }

    [CommandSwitch("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CommandSwitch("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CommandSwitch("--content-language")]
    public string? ContentLanguage { get; set; }

    [CommandSwitch("--content-length")]
    public long? ContentLength { get; set; }

    [CommandSwitch("--content-range")]
    public string? ContentRange { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--checksum-crc32")]
    public string? ChecksumCrc32 { get; set; }

    [CommandSwitch("--checksum-crc32-c")]
    public string? ChecksumCrc32C { get; set; }

    [CommandSwitch("--checksum-sha1")]
    public string? ChecksumSha1 { get; set; }

    [CommandSwitch("--checksum-sha256")]
    public string? ChecksumSha256 { get; set; }

    [CommandSwitch("--e-tag")]
    public string? ETag { get; set; }

    [CommandSwitch("--expires")]
    public long? Expires { get; set; }

    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [CommandSwitch("--last-modified")]
    public long? LastModified { get; set; }

    [CommandSwitch("--missing-meta")]
    public int? MissingMeta { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--object-lock-mode")]
    public string? ObjectLockMode { get; set; }

    [CommandSwitch("--object-lock-legal-hold-status")]
    public string? ObjectLockLegalHoldStatus { get; set; }

    [CommandSwitch("--object-lock-retain-until-date")]
    public long? ObjectLockRetainUntilDate { get; set; }

    [CommandSwitch("--parts-count")]
    public int? PartsCount { get; set; }

    [CommandSwitch("--replication-status")]
    public string? ReplicationStatus { get; set; }

    [CommandSwitch("--request-charged")]
    public string? RequestCharged { get; set; }

    [CommandSwitch("--restore")]
    public string? Restore { get; set; }

    [CommandSwitch("--server-side-encryption")]
    public string? ServerSideEncryption { get; set; }

    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--ssekms-key-id")]
    public string? SsekmsKeyId { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--storage-class")]
    public string? StorageClass { get; set; }

    [CommandSwitch("--tag-count")]
    public int? TagCount { get; set; }

    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}