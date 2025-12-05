using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "vpn-connections", "describe")]
public record GcloudEdgeCloudContainerVpnConnectionsDescribeOptions(
[property: CliArgument] string VpnConnection,
[property: CliArgument] string Location
) : GcloudOptions;