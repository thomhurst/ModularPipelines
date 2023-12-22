using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "show")]
public record AzPartnercenterMarketplaceOfferPlanShowOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions;