using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "list")]
public record AzNetworkVnetGatewayListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}