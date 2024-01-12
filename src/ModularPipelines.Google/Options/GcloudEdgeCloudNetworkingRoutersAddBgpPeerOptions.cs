using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "routers", "add-bgp-peer")]
public record GcloudEdgeCloudNetworkingRoutersAddBgpPeerOptions(
[property: PositionalArgument] string Router,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--interface")] string Interface,
[property: CommandSwitch("--peer-asn")] string PeerAsn,
[property: CommandSwitch("--peer-ipv4-range")] string PeerIpv4Range,
[property: CommandSwitch("--peer-name")] string PeerName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}