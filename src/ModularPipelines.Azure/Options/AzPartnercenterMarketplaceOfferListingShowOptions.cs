using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "listing", "show")]
public record AzPartnercenterMarketplaceOfferListingShowOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;