using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "spokes", "linked-vpn-tunnels", "create")]
public record GcloudNetworkConnectivitySpokesLinkedVpnTunnelsCreateOptions(
[property: PositionalArgument] string Spoke,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--hub")] string Hub,
[property: CommandSwitch("--vpn-tunnels")] string[] VpnTunnels
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--site-to-site-data-transfer")]
    public bool? SiteToSiteDataTransfer { get; set; }
}