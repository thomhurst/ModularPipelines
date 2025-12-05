using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "reservation-order", "change-directory")]
public record AzReservationsReservationOrderChangeDirectoryOptions(
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliOption("--destination-tenant-id")]
    public string? DestinationTenantId { get; set; }
}