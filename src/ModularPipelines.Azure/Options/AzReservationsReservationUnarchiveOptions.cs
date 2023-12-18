using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation", "unarchive")]
public record AzReservationsReservationUnarchiveOptions(
[property: CommandSwitch("--reservation-id")] string ReservationId,
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions;