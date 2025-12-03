using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan-order", "show")]
public record AzBillingBenefitsSavingsPlanOrderShowOptions(
[property: CliOption("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}