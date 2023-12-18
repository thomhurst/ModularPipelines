using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "fs", "list")]
public record AzStorageFsListOptions : AzOptions
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

    [BooleanCommandSwitch("--include-metadata")]
    public bool? IncludeMetadata { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }
}