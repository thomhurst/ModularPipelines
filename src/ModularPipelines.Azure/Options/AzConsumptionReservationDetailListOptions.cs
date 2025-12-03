using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("consumption", "reservation", "detail", "list")]
public record AzConsumptionReservationDetailListOptions(
[property: CliOption("--end-date")] string EndDate,
[property: CliOption("--reservation-order-id")] string ReservationOrderId,
[property: CliOption("--start-date")] string StartDate
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--reservation-id")]
    public string? ReservationId { get; set; }
}