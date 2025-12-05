using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing-benefits", "savings-plan-order", "savings-plan", "show")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanShowOptions(
[property: CliOption("--savings-plan-id")] string SavingsPlanId,
[property: CliOption("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}