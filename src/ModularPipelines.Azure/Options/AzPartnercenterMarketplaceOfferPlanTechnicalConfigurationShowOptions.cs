using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "technical-configuration", "show")]
public record AzPartnercenterMarketplaceOfferPlanTechnicalConfigurationShowOptions(
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--plan-id")] string PlanId
) : AzOptions
{
}