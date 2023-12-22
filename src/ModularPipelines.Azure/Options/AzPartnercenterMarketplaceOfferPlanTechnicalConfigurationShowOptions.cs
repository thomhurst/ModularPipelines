using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "technical-configuration", "show")]
public record AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationShowOptions(
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--plan-id")] string PlanId
) : AzOptions;