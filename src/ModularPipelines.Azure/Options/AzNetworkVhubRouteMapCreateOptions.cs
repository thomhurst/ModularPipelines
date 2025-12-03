using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "route-map", "create")]
public record AzNetworkVhubRouteMapCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliOption("--inbound-connection")]
    public string? InboundConnection { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--outbound-connections")]
    public string? OutboundConnections { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }
}