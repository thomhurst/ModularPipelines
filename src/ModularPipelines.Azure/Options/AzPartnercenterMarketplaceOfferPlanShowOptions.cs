using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "plan", "show")]
public record AzPartnercenterMarketplaceOfferPlanShowOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;