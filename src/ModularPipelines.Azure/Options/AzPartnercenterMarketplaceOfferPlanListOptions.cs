using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "plan", "list")]
public record AzPartnercenterMarketplaceOfferPlanListOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;