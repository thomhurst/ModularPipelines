using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "vpn-connections", "delete")]
public record GcloudEdgeCloudContainerVpnConnectionsDeleteOptions(
[property: CliArgument] string VpnConnection,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}