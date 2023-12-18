using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan-order", "savings-plan", "list")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanListOptions(
[property: CommandSwitch("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions;