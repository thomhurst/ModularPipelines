using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vhub", "route-table", "list")]
public record AzNetworkVhubRouteTableListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions;