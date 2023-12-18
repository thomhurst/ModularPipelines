using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "fabric", "update")]
public record AzNetworkfabricFabricUpdateOptions : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--fabric-asn")]
    public string? FabricAsn { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ipv4-prefix")]
    public string? Ipv4Prefix { get; set; }

    [CommandSwitch("--ipv6-prefix")]
    public string? Ipv6Prefix { get; set; }

    [CommandSwitch("--managed-network-config")]
    public string? ManagedNetworkConfig { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--rack-count")]
    public int? RackCount { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--server-count-per-rack")]
    public int? ServerCountPerRack { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--ts-config")]
    public string? TsConfig { get; set; }
}