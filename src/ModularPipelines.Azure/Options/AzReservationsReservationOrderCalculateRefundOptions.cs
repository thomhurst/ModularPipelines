using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation-order", "calculate-refund")]
public record AzReservationsReservationOrderCalculateRefundOptions(
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--quantity")]
    public string? Quantity { get; set; }

    [CommandSwitch("--reservation-id")]
    public string? ReservationId { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}

