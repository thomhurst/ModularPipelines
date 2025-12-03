using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan", "list")]
public record AzBillingBenefitsSavingsPlanListOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--selected-state")]
    public string? SelectedState { get; set; }

    [CliOption("--skiptoken")]
    public string? Skiptoken { get; set; }

    [CliOption("--take")]
    public string? Take { get; set; }
}