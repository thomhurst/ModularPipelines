using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("reservations", "reservation-order-id", "list")]
public record AzReservationsReservationOrderIdListOptions(
[property: CliOption("--subscription-id")] string SubscriptionId
) : AzOptions;