using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation", "archive")]
public record AzReservationsReservationArchiveOptions(
[property: CommandSwitch("--reservation-id")] string ReservationId,
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions;