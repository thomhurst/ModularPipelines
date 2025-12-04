using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "peering", "update")]
public record AzNetworkExpressRoutePeeringUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--advertised-public-prefixes")]
    public string? AdvertisedPublicPrefixes { get; set; }

    [CliOption("--circuit-name")]
    public string? CircuitName { get; set; }

    [CliOption("--customer-asn")]
    public string? CustomerAsn { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ip-version")]
    public string? IpVersion { get; set; }

    [CliOption("--legacy-mode")]
    public string? LegacyMode { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CliOption("--primary-peer-subnet")]
    public string? PrimaryPeerSubnet { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--route-filter")]
    public string? RouteFilter { get; set; }

    [CliOption("--routing-registry-name")]
    public string? RoutingRegistryName { get; set; }

    [CliOption("--secondary-peer-subnet")]
    public string? SecondaryPeerSubnet { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--shared-key")]
    public string? SharedKey { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vlan-id")]
    public string? VlanId { get; set; }
}