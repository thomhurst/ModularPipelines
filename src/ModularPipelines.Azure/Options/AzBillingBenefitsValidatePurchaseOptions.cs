using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing-benefits", "validate-purchase")]
public record AzBillingBenefitsValidatePurchaseOptions : AzOptions
{
    [CliOption("--benefits")]
    public string? Benefits { get; set; }
}