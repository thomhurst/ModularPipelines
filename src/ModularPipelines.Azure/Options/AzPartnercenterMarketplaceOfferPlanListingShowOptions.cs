using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "listing", "show")]
public record AzPartnercenterMarketplaceOfferPlanListingShowOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions;