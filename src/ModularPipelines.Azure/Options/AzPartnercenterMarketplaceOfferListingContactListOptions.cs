using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "contact", "list")]
public record AzPartnercenterMarketplaceOfferListingContactListOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions;