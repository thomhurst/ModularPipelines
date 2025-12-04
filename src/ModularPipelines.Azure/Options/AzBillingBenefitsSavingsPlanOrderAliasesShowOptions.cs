using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing-benefits", "savings-plan-order-aliases", "show")]
public record AzBillingBenefitsSavingsPlanOrderAliasesShowOptions(
[property: CliOption("--order-alias-name")] string OrderAliasName
) : AzOptions;