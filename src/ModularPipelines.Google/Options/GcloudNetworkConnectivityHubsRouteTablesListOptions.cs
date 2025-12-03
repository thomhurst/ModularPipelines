using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "route-tables", "list")]
public record GcloudNetworkConnectivityHubsRouteTablesListOptions(
[property: CliOption("--hub")] string Hub
) : GcloudOptions;