using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "approuting", "zone", "update")]
public record AzAksApproutingZoneUpdateOptions(
[property: CommandSwitch("--ids")] string Ids,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--attach-zones")]
    public bool? AttachZones { get; set; }
}