using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation-order", "change-directory")]
public record AzReservationsReservationOrderChangeDirectoryOptions(
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CommandSwitch("--destination-tenant-id")]
    public string? DestinationTenantId { get; set; }
}