using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "marketplace", "list")]
public record AzConsumptionMarketplaceListOptions : AzOptions
{
    [CommandSwitch("--billing-period-name")]
    public string? BillingPeriodName { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}