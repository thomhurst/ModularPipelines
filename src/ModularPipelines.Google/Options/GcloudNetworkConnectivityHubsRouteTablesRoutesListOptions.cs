using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "route-tables", "routes", "list")]
public record GcloudNetworkConnectivityHubsRouteTablesRoutesListOptions(
[property: CommandSwitch("--route_table")] string RouteTable,
[property: CommandSwitch("--hub")] string Hub
) : GcloudOptions;