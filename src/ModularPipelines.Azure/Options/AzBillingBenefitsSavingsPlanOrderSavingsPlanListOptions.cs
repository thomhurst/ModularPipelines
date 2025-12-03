using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan-order", "savings-plan", "list")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanListOptions(
[property: CliOption("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions;