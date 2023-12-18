using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("capacity", "reservation", "update")]
public record AzCapacityReservationUpdateOptions(
[property: CommandSwitch("--capacity-reservation-group")] string CapacityReservationGroup,
[property: CommandSwitch("--capacity-reservation-name")] string CapacityReservationName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

