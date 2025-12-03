using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file", "copy", "start")]
public record AzStorageFileCopyStartOptions(
[property: CliOption("--destination-path")] string DestinationPath,
[property: CliOption("--destination-share")] string DestinationShare
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

    [CliFlag("--disallow-source-trailing-dot")]
    public bool? DisallowSourceTrailingDot { get; set; }

    [CliFlag("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CliOption("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CliOption("--file-snapshot")]
    public string? FileSnapshot { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

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

    [CliOption("--source-path")]
    public string? SourcePath { get; set; }

    [CliOption("--source-sas")]
    public string? SourceSas { get; set; }

    [CliOption("--source-share")]
    public string? SourceShare { get; set; }

    [CliOption("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CliOption("--source-uri")]
    public string? SourceUri { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}