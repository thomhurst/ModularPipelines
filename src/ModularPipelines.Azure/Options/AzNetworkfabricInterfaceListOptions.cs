using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "interface", "list")]
public record AzNetworkfabricInterfaceListOptions(
[property: CommandSwitch("--device")] string Device,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}