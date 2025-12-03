using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob", "set-tier", "(storage-blob-preview", "extension)")]
public record AzStorageBlobSetTierStorageBlobPreviewExtensionOptions(
[property: CliOption("--tier")] string Tier
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

    [CliOption("--blob-url")]
    public string? BlobUrl { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--rehydrate-priority")]
    public string? RehydratePriority { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--tags-condition")]
    public string? TagsCondition { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }
}