using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "flexible-server", "replica", "create")]
public record AzPostgresFlexibleServerReplicaCreateOptions(
[property: CommandSwitch("--replica-name")] string ReplicaName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source-server")] string SourceServer
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--performance-tier")]
    public string? PerformanceTier { get; set; }

    [CommandSwitch("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CommandSwitch("--sku-name")]
    public string? SkuName { get; set; }

    [CommandSwitch("--storage-size")]
    public string? StorageSize { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}