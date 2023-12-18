using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "usage", "list")]
public record AzConsumptionUsageListOptions : AzOptions
{
    [CommandSwitch("--billing-period-name")]
    public string? BillingPeriodName { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--include-additional-properties")]
    public string? IncludeAdditionalProperties { get; set; }

    [CommandSwitch("--include-meter-details")]
    public string? IncludeMeterDetails { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--start-date")]
    public string? StartDate { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}

