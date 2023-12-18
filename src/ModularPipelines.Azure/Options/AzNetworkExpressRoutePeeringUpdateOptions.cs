using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering", "update")]
public record AzNetworkExpressRoutePeeringUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--advertised-public-prefixes")]
    public string? AdvertisedPublicPrefixes { get; set; }

    [CommandSwitch("--circuit-name")]
    public string? CircuitName { get; set; }

    [CommandSwitch("--customer-asn")]
    public string? CustomerAsn { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-version")]
    public string? IpVersion { get; set; }

    [CommandSwitch("--legacy-mode")]
    public string? LegacyMode { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CommandSwitch("--primary-peer-subnet")]
    public string? PrimaryPeerSubnet { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--route-filter")]
    public string? RouteFilter { get; set; }

    [CommandSwitch("--routing-registry-name")]
    public string? RoutingRegistryName { get; set; }

    [CommandSwitch("--secondary-peer-subnet")]
    public string? SecondaryPeerSubnet { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--shared-key")]
    public string? SharedKey { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--vlan-id")]
    public string? VlanId { get; set; }
}