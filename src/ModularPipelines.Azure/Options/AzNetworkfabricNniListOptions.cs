using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "nni", "list")]
public record AzNetworkfabricNniListOptions(
[property: CommandSwitch("--fabric")] string Fabric,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}