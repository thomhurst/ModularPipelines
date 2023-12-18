using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan", "list")]
public record AzBillingBenefitsSavingsPlanListOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--orderby")]
    public string? Orderby { get; set; }

    [CommandSwitch("--selected-state")]
    public string? SelectedState { get; set; }

    [CommandSwitch("--skiptoken")]
    public string? Skiptoken { get; set; }

    [CommandSwitch("--take")]
    public string? Take { get; set; }
}