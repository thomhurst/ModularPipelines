using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "copy", "start-batch")]
public record AzStorageFileCopyStartBatchOptions : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--destination-path")]
    public string? DestinationPath { get; set; }

    [CommandSwitch("--destination-share")]
    public string? DestinationShare { get; set; }

    [BooleanCommandSwitch("--disallow-source-trailing-dot")]
    public bool? DisallowSourceTrailingDot { get; set; }

    [BooleanCommandSwitch("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [BooleanCommandSwitch("--dryrun")]
    public bool? Dryrun { get; set; }

    [CommandSwitch("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--pattern")]
    public string? Pattern { get; set; }

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

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}