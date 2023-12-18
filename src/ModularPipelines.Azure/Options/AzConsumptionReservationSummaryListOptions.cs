using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "reservation", "summary", "list")]
public record AzConsumptionReservationSummaryListOptions(
[property: CommandSwitch("--grain")] string Grain,
[property: CommandSwitch("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--reservation-id")]
    public string? ReservationId { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }
}