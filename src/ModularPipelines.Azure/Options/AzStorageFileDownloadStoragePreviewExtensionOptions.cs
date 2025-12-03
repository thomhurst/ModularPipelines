using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file", "download", "(storage-preview", "extension)")]
public record AzStorageFileDownloadStoragePreviewExtensionOptions(
[property: CliOption("--path")] string Path,
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

    [CliOption("--dest")]
    public string? Dest { get; set; }

    [CliFlag("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CliOption("--end-range")]
    public string? EndRange { get; set; }

    [CliOption("--max-connections")]
    public string? MaxConnections { get; set; }

    [CliFlag("--no-progress")]
    public bool? NoProgress { get; set; }

    [CliOption("--open-mode")]
    public string? OpenMode { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--snapshot")]
    public string? Snapshot { get; set; }

    [CliOption("--start-range")]
    public string? StartRange { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--validate-content")]
    public string? ValidateContent { get; set; }
}