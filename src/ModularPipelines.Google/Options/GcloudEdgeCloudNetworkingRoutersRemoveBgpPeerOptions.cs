using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "routers", "remove-bgp-peer")]
public record GcloudEdgeCloudNetworkingRoutersRemoveBgpPeerOptions(
[property: CliArgument] string Router,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--peer-name")] string PeerName,
[property: CliOption("--peer-names")] string[] PeerNames
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}