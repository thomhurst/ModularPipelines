using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "vpn-connections", "describe")]
public record GcloudEdgeCloudContainerVpnConnectionsDescribeOptions(
[property: PositionalArgument] string VpnConnection,
[property: PositionalArgument] string Location
) : GcloudOptions;