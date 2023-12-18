using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan-order", "savings-plan", "validate-update")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanValidateUpdateOptions(
[property: CommandSwitch("--savings-plan-id")] string SavingsPlanId,
[property: CommandSwitch("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CommandSwitch("--benefits")]
    public string? Benefits { get; set; }
}