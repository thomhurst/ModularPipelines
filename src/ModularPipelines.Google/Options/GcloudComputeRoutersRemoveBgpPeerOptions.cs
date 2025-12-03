using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "remove-bgp-peer")]
public record GcloudComputeRoutersRemoveBgpPeerOptions(
[property: CliArgument] string Name,
[property: CliOption("--peer-name")] string PeerName,
[property: CliOption("--peer-names")] string[] PeerNames
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}