using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "approuting", "zone", "delete")]
public record AzAksApproutingZoneDeleteOptions(
[property: CommandSwitch("--ids")] string Ids,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}