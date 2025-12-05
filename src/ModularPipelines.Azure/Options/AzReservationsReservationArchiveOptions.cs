using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "reservation", "archive")]
public record AzReservationsReservationArchiveOptions(
[property: CliOption("--reservation-id")] string ReservationId,
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions;