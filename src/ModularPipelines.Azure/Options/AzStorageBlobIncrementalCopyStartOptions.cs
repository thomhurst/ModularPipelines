using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "incremental-copy", "start")]
public record AzStorageBlobIncrementalCopyStartOptions(
[property: CommandSwitch("--destination-blob")] string DestinationBlob,
[property: CommandSwitch("--destination-container")] string DestinationContainer
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--destination-if-match")]
    public string? DestinationIfMatch { get; set; }

    [CommandSwitch("--destination-if-modified-since")]
    public string? DestinationIfModifiedSince { get; set; }

    [CommandSwitch("--destination-if-none-match")]
    public string? DestinationIfNoneMatch { get; set; }

    [CommandSwitch("--destination-if-unmodified-since")]
    public string? DestinationIfUnmodifiedSince { get; set; }

    [CommandSwitch("--destination-lease-id")]
    public string? DestinationLeaseId { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--source-account-key")]
    public int? SourceAccountKey { get; set; }

    [CommandSwitch("--source-account-name")]
    public int? SourceAccountName { get; set; }

    [CommandSwitch("--source-blob")]
    public string? SourceBlob { get; set; }

    [CommandSwitch("--source-container")]
    public string? SourceContainer { get; set; }

    [CommandSwitch("--source-lease-id")]
    public string? SourceLeaseId { get; set; }

    [CommandSwitch("--source-sas")]
    public string? SourceSas { get; set; }

    [CommandSwitch("--source-snapshot")]
    public string? SourceSnapshot { get; set; }

    [CommandSwitch("--source-uri")]
    public string? SourceUri { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}