using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan-order", "savings-plan", "show")]
public record AzBillingBenefitsSavingsPlanOrderSavingsPlanShowOptions(
[property: CommandSwitch("--savings-plan-id")] string SavingsPlanId,
[property: CommandSwitch("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}