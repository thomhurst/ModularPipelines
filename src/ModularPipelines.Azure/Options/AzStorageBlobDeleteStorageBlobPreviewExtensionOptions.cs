using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "blob", "delete", "(storage-blob-preview", "extension)")]
public record AzStorageBlobDeleteStorageBlobPreviewExtensionOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--blob-url")]
    public string? BlobUrl { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--delete-snapshots")]
    public string? DeleteSnapshots { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CliOption("--lease-id")]
    public string? LeaseId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--snapshot")]
    public string? Snapshot { get; set; }

    [CliOption("--tags-condition")]
    public string? TagsCondition { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }
}