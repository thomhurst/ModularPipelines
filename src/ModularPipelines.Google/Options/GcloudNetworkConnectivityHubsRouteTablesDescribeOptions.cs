using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "route-tables", "describe")]
public record GcloudNetworkConnectivityHubsRouteTablesDescribeOptions(
[property: CliArgument] string RouteTable,
[property: CliArgument] string Hub
) : GcloudOptions;