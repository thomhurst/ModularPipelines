using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "routeserver", "peering", "list")]
public record AzNetworkRouteserverPeeringListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--routeserver")] string Routeserver
) : AzOptions;