using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "upload", "(storage-preview", "extension)")]
public record AzStorageFileUploadStoragePreviewExtensionOptions(
[property: CliOption("--share-name")] string ShareName,
[property: CliOption("--source")] string Source
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

    [CliOption("--max-connections")]
    public string? MaxConnections { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliFlag("--no-progress")]
    public bool? NoProgress { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--validate-content")]
    public bool? ValidateContent { get; set; }
}