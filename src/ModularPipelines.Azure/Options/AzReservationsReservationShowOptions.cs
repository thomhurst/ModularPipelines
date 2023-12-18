using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation", "show")]
public record AzReservationsReservationShowOptions(
[property: CommandSwitch("--reservation-id")] string ReservationId,
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}