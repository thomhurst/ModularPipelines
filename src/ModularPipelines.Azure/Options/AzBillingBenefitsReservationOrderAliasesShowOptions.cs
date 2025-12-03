using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "reservation-order-aliases", "show")]
public record AzBillingBenefitsReservationOrderAliasesShowOptions(
[property: CliOption("--order-alias-name")] string OrderAliasName
) : AzOptions;