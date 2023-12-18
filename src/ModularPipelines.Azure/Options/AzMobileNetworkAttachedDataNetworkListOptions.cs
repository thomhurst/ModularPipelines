using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "attached-data-network", "list")]
public record AzMobileNetworkAttachedDataNetworkListOptions(
[property: CommandSwitch("--pccp-name")] string PccpName,
[property: CommandSwitch("--pcdp-name")] string PcdpName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}