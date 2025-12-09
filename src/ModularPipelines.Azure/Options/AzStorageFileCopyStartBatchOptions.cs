using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "file", "copy", "start-batch")]
public record AzStorageFileCopyStartBatchOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--destination-path")]
    public string? DestinationPath { get; set; }

    [CliOption("--destination-share")]
    public string? DestinationShare { get; set; }

    [CliFlag("--disallow-source-trailing-dot")]
    public bool? DisallowSourceTrailingDot { get; set; }

    [CliFlag("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CliFlag("--dryrun")]
    public bool? Dryrun { get; set; }

    [CliOption("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--pattern")]
    public string? Pattern { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--source-account-key")]
    public int? SourceAccountKey { get; set; }

    [CliOption("--source-account-name")]
    public int? SourceAccountName { get; set; }

    [CliOption("--source-container")]
    public string? SourceContainer { get; set; }

    [CliOption("--source-sas")]
    public string? SourceSas { get; set; }

    [CliOption("--source-share")]
    public string? SourceShare { get; set; }

    [CliOption("--source-uri")]
    public string? SourceUri { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}