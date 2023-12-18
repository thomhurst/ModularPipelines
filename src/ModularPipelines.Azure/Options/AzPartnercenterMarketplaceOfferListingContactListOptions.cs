using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "contact", "list")]
public record AzPartnercenterMarketplaceOfferListingContactListOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions
{
}