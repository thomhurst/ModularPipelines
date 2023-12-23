using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3api", "copy-object")]
public record AwsS3apiCopyObjectOptions(
[property: CommandSwitch("--bucket")] string Bucket,
[property: CommandSwitch("--copy-source")] string CopySource,
[property: CommandSwitch("--key")] string Key
) : AwsOptions
{
    [CommandSwitch("--acl")]
    public string? Acl { get; set; }

    [CommandSwitch("--cache-control")]
    public string? CacheControl { get; set; }

    [CommandSwitch("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CommandSwitch("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CommandSwitch("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CommandSwitch("--content-language")]
    public string? ContentLanguage { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--copy-source-if-match")]
    public string? CopySourceIfMatch { get; set; }

    [CommandSwitch("--copy-source-if-modified-since")]
    public long? CopySourceIfModifiedSince { get; set; }

    [CommandSwitch("--copy-source-if-none-match")]
    public string? CopySourceIfNoneMatch { get; set; }

    [CommandSwitch("--copy-source-if-unmodified-since")]
    public long? CopySourceIfUnmodifiedSince { get; set; }

    [CommandSwitch("--expires")]
    public long? Expires { get; set; }

    [CommandSwitch("--grant-full-control")]
    public string? GrantFullControl { get; set; }

    [CommandSwitch("--grant-read")]
    public string? GrantRead { get; set; }

    [CommandSwitch("--grant-read-acp")]
    public string? GrantReadAcp { get; set; }

    [CommandSwitch("--grant-write-acp")]
    public string? GrantWriteAcp { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--metadata-directive")]
    public string? MetadataDirective { get; set; }

    [CommandSwitch("--tagging-directive")]
    public string? TaggingDirective { get; set; }

    [CommandSwitch("--server-side-encryption")]
    public string? ServerSideEncryption { get; set; }

    [CommandSwitch("--storage-class")]
    public string? StorageClass { get; set; }

    [CommandSwitch("--website-redirect-location")]
    public string? WebsiteRedirectLocation { get; set; }

    [CommandSwitch("--sse-customer-algorithm")]
    public string? SseCustomerAlgorithm { get; set; }

    [CommandSwitch("--sse-customer-key")]
    public string? SseCustomerKey { get; set; }

    [CommandSwitch("--sse-customer-key-md5")]
    public string? SseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--ssekms-key-id")]
    public string? SsekmsKeyId { get; set; }

    [CommandSwitch("--ssekms-encryption-context")]
    public string? SsekmsEncryptionContext { get; set; }

    [CommandSwitch("--copy-source-sse-customer-algorithm")]
    public string? CopySourceSseCustomerAlgorithm { get; set; }

    [CommandSwitch("--copy-source-sse-customer-key")]
    public string? CopySourceSseCustomerKey { get; set; }

    [CommandSwitch("--copy-source-sse-customer-key-md5")]
    public string? CopySourceSseCustomerKeyMd5 { get; set; }

    [CommandSwitch("--request-payer")]
    public string? RequestPayer { get; set; }

    [CommandSwitch("--tagging")]
    public string? Tagging { get; set; }

    [CommandSwitch("--object-lock-mode")]
    public string? ObjectLockMode { get; set; }

    [CommandSwitch("--object-lock-retain-until-date")]
    public long? ObjectLockRetainUntilDate { get; set; }

    [CommandSwitch("--object-lock-legal-hold-status")]
    public string? ObjectLockLegalHoldStatus { get; set; }

    [CommandSwitch("--expected-bucket-owner")]
    public string? ExpectedBucketOwner { get; set; }

    [CommandSwitch("--expected-source-bucket-owner")]
    public string? ExpectedSourceBucketOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}