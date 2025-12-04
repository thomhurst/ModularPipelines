using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "listing", "show")]
public record AzPartnercenterMarketplaceOfferListingShowOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;