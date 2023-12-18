using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("reservations", "reservation-order-id", "list")]
public record AzReservationsReservationOrderIdListOptions(
[property: CommandSwitch("--subscription-id")] string SubscriptionId
) : AzOptions
{
}

