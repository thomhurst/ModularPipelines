using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "approuting", "zone", "add")]
public record AzAksApproutingZoneAddOptions(
[property: CommandSwitch("--ids")] string Ids,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--attach-zones")]
    public bool? AttachZones { get; set; }
}