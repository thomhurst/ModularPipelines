using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "directory", "metadata", "update", "(storage-preview", "extension)")]
public record AzStorageDirectoryMetadataUpdateStoragePreviewExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--share-name")] string ShareName
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliFlag("--backup-intent")]
    public bool? BackupIntent { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--snapshot")]
    public string? Snapshot { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}