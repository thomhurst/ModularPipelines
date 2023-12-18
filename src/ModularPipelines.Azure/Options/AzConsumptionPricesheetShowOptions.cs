using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "pricesheet", "show")]
public record AzConsumptionPricesheetShowOptions : AzOptions
{
    [CommandSwitch("--billing-period-name")]
    public string? BillingPeriodName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--include-meter-details")]
    public string? IncludeMeterDetails { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}