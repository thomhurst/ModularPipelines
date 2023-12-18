using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "uri", "list")]
public record AzPartnercenterMarketplaceOfferListingUriListOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions;