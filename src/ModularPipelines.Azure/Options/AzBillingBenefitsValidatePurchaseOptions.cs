using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "validate-purchase")]
public record AzBillingBenefitsValidatePurchaseOptions : AzOptions
{
    [CommandSwitch("--benefits")]
    public string? Benefits { get; set; }
}