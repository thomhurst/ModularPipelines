using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("capacity", "reservation", "group", "update")]
public record AzCapacityReservationGroupUpdateOptions(
[property: CommandSwitch("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}