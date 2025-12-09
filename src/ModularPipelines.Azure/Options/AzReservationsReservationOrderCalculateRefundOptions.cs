using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "reservation-order", "calculate-refund")]
public record AzReservationsReservationOrderCalculateRefundOptions(
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--quantity")]
    public string? Quantity { get; set; }

    [CliOption("--reservation-id")]
    public string? ReservationId { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}