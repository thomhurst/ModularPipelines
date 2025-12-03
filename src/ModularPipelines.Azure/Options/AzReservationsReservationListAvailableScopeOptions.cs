using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("reservations", "reservation", "list-available-scope")]
public record AzReservationsReservationListAvailableScopeOptions(
[property: CliOption("--reservation-id")] string ReservationId,
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }
}