using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "reservation", "detail", "list")]
public record AzConsumptionReservationDetailListOptions(
[property: CommandSwitch("--end-date")] string EndDate,
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId,
[property: CommandSwitch("--start-date")] string StartDate
) : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--reservation-id")]
    public string? ReservationId { get; set; }
}