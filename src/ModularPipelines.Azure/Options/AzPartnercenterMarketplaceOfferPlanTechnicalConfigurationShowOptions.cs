using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "plan", "technical-configuration", "show")]
public record AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationShowOptions(
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--plan-id")] string PlanId
) : AzOptions;