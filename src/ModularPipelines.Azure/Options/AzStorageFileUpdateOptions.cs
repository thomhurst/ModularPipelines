using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "file", "update")]
public record AzStorageFileUpdateOptions(
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

    [CliFlag("--clear-content-settings")]
    public bool? ClearContentSettings { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--content-cache")]
    public string? ContentCache { get; set; }

    [CliOption("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CliOption("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CliOption("--content-language")]
    public string? ContentLanguage { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliFlag("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CliOption("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}