using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "routers", "add-bgp-peer")]
public record GcloudEdgeCloudNetworkingRoutersAddBgpPeerOptions(
[property: CliArgument] string Router,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--interface")] string Interface,
[property: CliOption("--peer-asn")] string PeerAsn,
[property: CliOption("--peer-ipv4-range")] string PeerIpv4Range,
[property: CliOption("--peer-name")] string PeerName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}