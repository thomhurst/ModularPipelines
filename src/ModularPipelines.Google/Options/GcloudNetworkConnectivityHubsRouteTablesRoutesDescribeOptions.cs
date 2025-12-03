using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "route-tables", "routes", "describe")]
public record GcloudNetworkConnectivityHubsRouteTablesRoutesDescribeOptions(
[property: CliArgument] string Route,
[property: CliArgument] string Hub,
[property: CliArgument] string RouteTable
) : GcloudOptions;