using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vhub", "route-map", "list")]
public record AzNetworkVhubRouteMapListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions;