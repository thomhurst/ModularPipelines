using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation", "merge")]
public record AzReservationsReservationMergeOptions(
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sources")]
    public string? Sources { get; set; }
}