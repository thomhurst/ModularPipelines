using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "peering", "create")]
public record AzNetworkExpressRoutePeeringCreateOptions(
[property: CliOption("--circuit-name")] string CircuitName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--advertised-public-prefixes")]
    public string? AdvertisedPublicPrefixes { get; set; }

    [CliOption("--customer-asn")]
    public string? CustomerAsn { get; set; }

    [CliOption("--ip-version")]
    public string? IpVersion { get; set; }

    [CliOption("--legacy-mode")]
    public string? LegacyMode { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CliOption("--peering-type")]
    public string? PeeringType { get; set; }

    [CliOption("--primary-peer-subnet")]
    public string? PrimaryPeerSubnet { get; set; }

    [CliOption("--route-filter")]
    public string? RouteFilter { get; set; }

    [CliOption("--routing-registry-name")]
    public string? RoutingRegistryName { get; set; }

    [CliOption("--secondary-peer-subnet")]
    public string? SecondaryPeerSubnet { get; set; }

    [CliOption("--shared-key")]
    public string? SharedKey { get; set; }

    [CliOption("--vlan-id")]
    public string? VlanId { get; set; }
}