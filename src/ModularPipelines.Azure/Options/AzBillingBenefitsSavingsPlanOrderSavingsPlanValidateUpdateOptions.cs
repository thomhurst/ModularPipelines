using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan-order", "savings-plan", "validate-update")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanValidateUpdateOptions(
[property: CliOption("--savings-plan-id")] string SavingsPlanId,
[property: CliOption("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CliOption("--benefits")]
    public string? Benefits { get; set; }
}