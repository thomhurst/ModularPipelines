using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation", "list-available-scope")]
public record AzReservationsReservationListAvailableScopeOptions(
[property: CommandSwitch("--reservation-id")] string ReservationId,
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--scopes")]
    public string? Scopes { get; set; }
}

