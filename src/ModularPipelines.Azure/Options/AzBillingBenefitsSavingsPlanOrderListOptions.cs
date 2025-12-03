using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan-order", "list")]
public record AzBillingBenefitsSavingsPlanOrderListOptions : AzOptions;