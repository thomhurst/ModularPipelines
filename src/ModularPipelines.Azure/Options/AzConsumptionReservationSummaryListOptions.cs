using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("consumption", "reservation", "summary", "list")]
public record AzConsumptionReservationSummaryListOptions(
[property: CliOption("--grain")] string Grain,
[property: CliOption("--reservation-order-id")] string ReservationOrderId
) : AzOptions
{
    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--reservation-id")]
    public string? ReservationId { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }
}