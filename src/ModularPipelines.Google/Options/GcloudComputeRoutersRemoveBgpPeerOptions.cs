using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "remove-bgp-peer")]
public record GcloudComputeRoutersRemoveBgpPeerOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--peer-name")] string PeerName,
[property: CommandSwitch("--peer-names")] string[] PeerNames
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}