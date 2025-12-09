using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "reservation", "show")]
public record AzReservationsReservationShowOptions(
[property: CliOption("--reservation-id")] string ReservationId,
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}