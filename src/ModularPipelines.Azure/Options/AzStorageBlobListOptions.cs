using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "list")]
public record AzStorageBlobListOptions(
[property: CommandSwitch("--container-name")] string ContainerName
) : AzOptions
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

    [CommandSwitch("--delimiter")]
    public string? Delimiter { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--num-results")]
    public string? NumResults { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}