using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob", "copy", "start", "(storage-blob-preview", "extension)")]
public record AzStorageBlobCopyStartStorageBlobPreviewExtensionOptions(
[property: CliOption("--destination-blob")] string DestinationBlob,
[property: CliOption("--destination-container")] string DestinationContainer
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--destination-blob-type")]
    public string? DestinationBlobType { get; set; }

    [CliOption("--destination-if-match")]
    public string? DestinationIfMatch { get; set; }

    [CliOption("--destination-if-modified-since")]
    public string? DestinationIfModifiedSince { get; set; }

    [CliOption("--destination-if-none-match")]
    public string? DestinationIfNoneMatch { get; set; }

    [CliOption("--destination-if-unmodified-since")]
    public string? DestinationIfUnmodifiedSince { get; set; }

    [CliOption("--destination-lease-id")]
    public string? DestinationLeaseId { get; set; }

    [CliOption("--destination-tags-condition")]
    public string? DestinationTagsCondition { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--rehydrate-priority")]
    public string? RehydratePriority { get; set; }

    [CliFlag("--requires-sync")]
    public bool? RequiresSync { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--source-account-key")]
    public int? SourceAccountKey { get; set; }

    [CliOption("--source-account-name")]
    public int? SourceAccountName { get; set; }

    [CliOption("--source-blob")]
    public string? SourceBlob { get; set; }

    [CliOption("--source-container")]
    public string? SourceContainer { get; set; }

    [CliOption("--source-if-match")]
    public string? SourceIfMatch { get; set; }

    [CliOption("--source-if-modified-since")]
    public string? SourceIfModifiedSince { get; set; }

    [CliOption("--source-if-none-match")]
    public string? SourceIfNoneMatch { get; set; }

    [CliOption("--source-if-unmodified-since")]
    public string? SourceIfUnmodifiedSince { get; set; }

    [CliOption("--source-lease-id")]
    public string? SourceLeaseId { get; set; }

    [CliOption("--source-path")]
    public string? SourcePath { get; set; }

    [CliOption("--source-sas")]
    public string? SourceSas { get; set; }

    [CliOption("--source-share")]
    public string? SourceShare { get; set; }

    [CliOption("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CliOption("--source-tags-condition")]
    public string? SourceTagsCondition { get; set; }

    [CliOption("--source-uri")]
    public string? SourceUri { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}