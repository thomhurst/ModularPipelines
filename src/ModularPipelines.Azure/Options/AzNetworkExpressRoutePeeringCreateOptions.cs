using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering", "create")]
public record AzNetworkExpressRoutePeeringCreateOptions(
[property: CommandSwitch("--circuit-name")] string CircuitName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--advertised-public-prefixes")]
    public string? AdvertisedPublicPrefixes { get; set; }

    [CommandSwitch("--customer-asn")]
    public string? CustomerAsn { get; set; }

    [CommandSwitch("--ip-version")]
    public string? IpVersion { get; set; }

    [CommandSwitch("--legacy-mode")]
    public string? LegacyMode { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CommandSwitch("--peering-type")]
    public string? PeeringType { get; set; }

    [CommandSwitch("--primary-peer-subnet")]
    public string? PrimaryPeerSubnet { get; set; }

    [CommandSwitch("--route-filter")]
    public string? RouteFilter { get; set; }

    [CommandSwitch("--routing-registry-name")]
    public string? RoutingRegistryName { get; set; }

    [CommandSwitch("--secondary-peer-subnet")]
    public string? SecondaryPeerSubnet { get; set; }

    [CommandSwitch("--shared-key")]
    public string? SharedKey { get; set; }

    [CommandSwitch("--vlan-id")]
    public string? VlanId { get; set; }
}