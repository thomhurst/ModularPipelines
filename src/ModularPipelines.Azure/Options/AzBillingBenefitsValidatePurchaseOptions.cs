using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "validate-purchase")]
public record AzBillingBenefitsValidatePurchaseOptions : AzOptions
{
    [CliOption("--benefits")]
    public string? Benefits { get; set; }
}