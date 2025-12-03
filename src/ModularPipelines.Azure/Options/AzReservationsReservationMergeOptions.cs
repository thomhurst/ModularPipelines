using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("reservations", "reservation", "merge")]
public record AzReservationsReservationMergeOptions(
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sources")]
    public string? Sources { get; set; }
}