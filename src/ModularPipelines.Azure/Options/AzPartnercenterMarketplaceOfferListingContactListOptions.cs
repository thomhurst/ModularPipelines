using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "listing", "contact", "list")]
public record AzPartnercenterMarketplaceOfferListingContactListOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;