using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan-order-aliases", "show")]
public record AzBillingBenefitsSavingsPlanOrderAliasesShowOptions(
[property: CliOption("--order-alias-name")] string OrderAliasName
) : AzOptions;