using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-connection", "peering", "create")]
public record AzNetworkCrossConnectionPeeringCreateOptions(
[property: CommandSwitch("--cross-connection-name")] string CrossConnectionName,
[property: CommandSwitch("--peer-asn")] string PeerAsn,
[property: CommandSwitch("--peering-type")] string PeeringType,
[property: CommandSwitch("--primary-peer-subnet")] string PrimaryPeerSubnet,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--secondary-peer-subnet")] string SecondaryPeerSubnet,
[property: CommandSwitch("--vlan-id")] string VlanId
) : AzOptions
{
    [CommandSwitch("--advertised-public-prefixes")]
    public string? AdvertisedPublicPrefixes { get; set; }

    [CommandSwitch("--customer-asn")]
    public string? CustomerAsn { get; set; }

    [CommandSwitch("--routing-registry-name")]
    public string? RoutingRegistryName { get; set; }

    [CommandSwitch("--shared-key")]
    public string? SharedKey { get; set; }
}