using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet-gateway", "list")]
public record AzNetworkVnetGatewayListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;