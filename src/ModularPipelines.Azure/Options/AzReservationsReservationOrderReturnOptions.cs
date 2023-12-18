using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation-order", "return")]
public record AzReservationsReservationOrderReturnOptions(
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--quantity")]
    public string? Quantity { get; set; }

    [CommandSwitch("--reservation-id")]
    public string? ReservationId { get; set; }

    [CommandSwitch("--return-reason")]
    public string? ReturnReason { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--session-id")]
    public string? SessionId { get; set; }
}