using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "route-tables", "list")]
public record GcloudNetworkConnectivityHubsRouteTablesListOptions(
[property: CommandSwitch("--hub")] string Hub
) : GcloudOptions;