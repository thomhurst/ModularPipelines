using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan-order", "show")]
public record AzBillingBenefitsSavingsPlanOrderShowOptions(
[property: CommandSwitch("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}