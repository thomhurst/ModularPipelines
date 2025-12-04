using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing-benefits", "savings-plan-order", "elevate")]
public record AzBillingBenefitsSavingsPlanOrderElevateOptions(
[property: CliOption("--savings-plan-order-id")] string SavingsPlanOrderId
) : AzOptions;