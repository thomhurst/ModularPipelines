using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "route-tables", "routes", "describe")]
public record GcloudNetworkConnectivityHubsRouteTablesRoutesDescribeOptions(
[property: PositionalArgument] string Route,
[property: PositionalArgument] string Hub,
[property: PositionalArgument] string RouteTable
) : GcloudOptions;