using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation-order", "show")]
public record AzReservationsReservationOrderShowOptions(
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}