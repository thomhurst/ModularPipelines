using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file", "resize", "(storage-preview", "extension)")]
public record AzStorageFileResizeStoragePreviewExtensionOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--share-name")] string ShareName,
[property: CliOption("--size")] string Size
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

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}