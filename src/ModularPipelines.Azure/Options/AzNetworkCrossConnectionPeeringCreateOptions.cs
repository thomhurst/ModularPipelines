using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "cross-connection", "peering", "create")]
public record AzNetworkCrossConnectionPeeringCreateOptions(
[property: CliOption("--cross-connection-name")] string CrossConnectionName,
[property: CliOption("--peer-asn")] string PeerAsn,
[property: CliOption("--peering-type")] string PeeringType,
[property: CliOption("--primary-peer-subnet")] string PrimaryPeerSubnet,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secondary-peer-subnet")] string SecondaryPeerSubnet,
[property: CliOption("--vlan-id")] string VlanId
) : AzOptions
{
    [CliOption("--advertised-public-prefixes")]
    public string? AdvertisedPublicPrefixes { get; set; }

    [CliOption("--customer-asn")]
    public string? CustomerAsn { get; set; }

    [CliOption("--routing-registry-name")]
    public string? RoutingRegistryName { get; set; }

    [CliOption("--shared-key")]
    public string? SharedKey { get; set; }
}