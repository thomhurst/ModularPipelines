using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "uri", "list")]
public record AzPartnercenterMarketplaceOfferListingUriListOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions
{
}