using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "listing", "uri", "list")]
public record AzPartnercenterMarketplaceOfferListingUriListOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;