using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "blob", "copy", "start-batch")]
public record AzStorageBlobCopyStartBatchOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--destination-blob-type")]
    public string? DestinationBlobType { get; set; }

    [CliOption("--destination-container")]
    public string? DestinationContainer { get; set; }

    [CliOption("--destination-path")]
    public string? DestinationPath { get; set; }

    [CliFlag("--dryrun")]
    public bool? Dryrun { get; set; }

    [CliOption("--pattern")]
    public string? Pattern { get; set; }

    [CliOption("--rehydrate-priority")]
    public string? RehydratePriority { get; set; }

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

    [CliOption("--tier")]
    public string? Tier { get; set; }
}