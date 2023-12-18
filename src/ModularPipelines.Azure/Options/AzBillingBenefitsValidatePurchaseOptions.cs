using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "validate-purchase")]
public record AzBillingBenefitsValidatePurchaseOptions : AzOptions
{
    [CommandSwitch("--benefits")]
    public string? Benefits { get; set; }
}