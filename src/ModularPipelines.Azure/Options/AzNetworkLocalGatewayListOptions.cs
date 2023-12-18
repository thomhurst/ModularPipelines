using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "local-gateway", "list")]
public record AzNetworkLocalGatewayListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}