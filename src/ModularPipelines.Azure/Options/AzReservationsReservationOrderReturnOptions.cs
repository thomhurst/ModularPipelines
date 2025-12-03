using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("reservations", "reservation-order", "return")]
public record AzReservationsReservationOrderReturnOptions(
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--quantity")]
    public string? Quantity { get; set; }

    [CliOption("--reservation-id")]
    public string? ReservationId { get; set; }

    [CliOption("--return-reason")]
    public string? ReturnReason { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--session-id")]
    public string? SessionId { get; set; }
}