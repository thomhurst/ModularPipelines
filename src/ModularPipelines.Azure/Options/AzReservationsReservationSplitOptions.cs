using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "reservation", "split")]
public record AzReservationsReservationSplitOptions(
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--quantities")]
    public string? Quantities { get; set; }

    [CliOption("--reservation-id")]
    public string? ReservationId { get; set; }
}