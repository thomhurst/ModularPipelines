using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "spokes", "linked-vpn-tunnels", "create")]
public record GcloudNetworkConnectivitySpokesLinkedVpnTunnelsCreateOptions(
[property: CliArgument] string Spoke,
[property: CliArgument] string Region,
[property: CliOption("--hub")] string Hub,
[property: CliOption("--vpn-tunnels")] string[] VpnTunnels
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--site-to-site-data-transfer")]
    public bool? SiteToSiteDataTransfer { get; set; }
}