using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("consumption", "usage", "list")]
public record AzConsumptionUsageListOptions : AzOptions
{
    [CliOption("--billing-period-name")]
    public string? BillingPeriodName { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--include-additional-properties")]
    public string? IncludeAdditionalProperties { get; set; }

    [CliOption("--include-meter-details")]
    public string? IncludeMeterDetails { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--start-date")]
    public string? StartDate { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}