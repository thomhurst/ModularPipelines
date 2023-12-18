using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "set-tier")]
public record AzStorageBlobSetTierOptions(
[property: CommandSwitch("--tier")] string Tier
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

    [CommandSwitch("--blob-url")]
    public string? BlobUrl { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--rehydrate-priority")]
    public string? RehydratePriority { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}