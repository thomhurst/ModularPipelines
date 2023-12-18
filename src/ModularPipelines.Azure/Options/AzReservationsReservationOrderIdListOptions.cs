using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation-order-id", "list")]
public record AzReservationsReservationOrderIdListOptions(
[property: CommandSwitch("--subscription-id")] string SubscriptionId
) : AzOptions;