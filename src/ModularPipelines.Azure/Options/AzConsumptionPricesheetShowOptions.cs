using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("consumption", "pricesheet", "show")]
public record AzConsumptionPricesheetShowOptions : AzOptions
{
    [CliOption("--billing-period-name")]
    public string? BillingPeriodName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--include-meter-details")]
    public string? IncludeMeterDetails { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}