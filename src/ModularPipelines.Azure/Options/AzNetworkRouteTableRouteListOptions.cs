using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "route-table", "route", "list")]
public record AzNetworkRouteTableRouteListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-table-name")] string RouteTableName
) : AzOptions;