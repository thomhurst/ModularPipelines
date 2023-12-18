using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("capacity", "reservation", "group", "show")]
public record AzCapacityReservationGroupShowOptions(
[property: CommandSwitch("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--instance-view")]
    public string? InstanceView { get; set; }
}