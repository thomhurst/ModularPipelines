using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "copy", "start")]
public record AzStorageBlobCopyStartOptions(
[property: CommandSwitch("--destination-blob")] string DestinationBlob,
[property: CommandSwitch("--destination-container")] string DestinationContainer
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--destination-blob-type")]
    public string? DestinationBlobType { get; set; }

    [CommandSwitch("--destination-if-match")]
    public string? DestinationIfMatch { get; set; }

    [CommandSwitch("--destination-if-modified-since")]
    public string? DestinationIfModifiedSince { get; set; }

    [CommandSwitch("--destination-if-none-match")]
    public string? DestinationIfNoneMatch { get; set; }

    [CommandSwitch("--destination-if-unmodified-since")]
    public string? DestinationIfUnmodifiedSince { get; set; }

    [CommandSwitch("--destination-lease-id")]
    public string? DestinationLeaseId { get; set; }

    [CommandSwitch("--destination-tags-condition")]
    public string? DestinationTagsCondition { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--rehydrate-priority")]
    public string? RehydratePriority { get; set; }

    [BooleanCommandSwitch("--requires-sync")]
    public bool? RequiresSync { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--source-account-key")]
    public int? SourceAccountKey { get; set; }

    [CommandSwitch("--source-account-name")]
    public int? SourceAccountName { get; set; }

    [CommandSwitch("--source-blob")]
    public string? SourceBlob { get; set; }

    [CommandSwitch("--source-container")]
    public string? SourceContainer { get; set; }

    [CommandSwitch("--source-if-match")]
    public string? SourceIfMatch { get; set; }

    [CommandSwitch("--source-if-modified-since")]
    public string? SourceIfModifiedSince { get; set; }

    [CommandSwitch("--source-if-none-match")]
    public string? SourceIfNoneMatch { get; set; }

    [CommandSwitch("--source-if-unmodified-since")]
    public string? SourceIfUnmodifiedSince { get; set; }

    [CommandSwitch("--source-lease-id")]
    public string? SourceLeaseId { get; set; }

    [CommandSwitch("--source-path")]
    public string? SourcePath { get; set; }

    [CommandSwitch("--source-sas")]
    public string? SourceSas { get; set; }

    [CommandSwitch("--source-share")]
    public string? SourceShare { get; set; }

    [CommandSwitch("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CommandSwitch("--source-tags-condition")]
    public string? SourceTagsCondition { get; set; }

    [CommandSwitch("--source-uri")]
    public string? SourceUri { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}