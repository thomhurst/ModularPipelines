using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "route-tables", "routes", "list")]
public record GcloudNetworkConnectivityHubsRouteTablesRoutesListOptions(
[property: CliOption("--route_table")] string RouteTable,
[property: CliOption("--hub")] string Hub
) : GcloudOptions;