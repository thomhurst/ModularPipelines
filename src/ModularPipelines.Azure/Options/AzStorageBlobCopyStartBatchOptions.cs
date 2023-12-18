using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "copy", "start-batch")]
public record AzStorageBlobCopyStartBatchOptions : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--destination-blob-type")]
    public string? DestinationBlobType { get; set; }

    [CommandSwitch("--destination-container")]
    public string? DestinationContainer { get; set; }

    [CommandSwitch("--destination-path")]
    public string? DestinationPath { get; set; }

    [BooleanCommandSwitch("--dryrun")]
    public bool? Dryrun { get; set; }

    [CommandSwitch("--pattern")]
    public string? Pattern { get; set; }

    [CommandSwitch("--rehydrate-priority")]
    public string? RehydratePriority { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--source-account-key")]
    public int? SourceAccountKey { get; set; }

    [CommandSwitch("--source-account-name")]
    public int? SourceAccountName { get; set; }

    [CommandSwitch("--source-container")]
    public string? SourceContainer { get; set; }

    [CommandSwitch("--source-sas")]
    public string? SourceSas { get; set; }

    [CommandSwitch("--source-share")]
    public string? SourceShare { get; set; }

    [CommandSwitch("--source-uri")]
    public string? SourceUri { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }
}