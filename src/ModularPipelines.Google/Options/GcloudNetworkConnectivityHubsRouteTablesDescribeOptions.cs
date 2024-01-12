using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "route-tables", "describe")]
public record GcloudNetworkConnectivityHubsRouteTablesDescribeOptions(
[property: PositionalArgument] string RouteTable,
[property: PositionalArgument] string Hub
) : GcloudOptions;