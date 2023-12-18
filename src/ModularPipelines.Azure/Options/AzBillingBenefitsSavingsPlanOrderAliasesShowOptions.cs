using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing-benefits", "savings-plan-order-aliases", "show")]
public record AzBillingBenefitsSavingsPlanOrderAliasesShowOptions(
[property: CommandSwitch("--order-alias-name")] string OrderAliasName
) : AzOptions
{
}