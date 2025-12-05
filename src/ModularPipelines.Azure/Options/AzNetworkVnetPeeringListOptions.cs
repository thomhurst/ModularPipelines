using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet", "peering", "list")]
public record AzNetworkVnetPeeringListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet-name")] string VnetName
) : AzOptions;