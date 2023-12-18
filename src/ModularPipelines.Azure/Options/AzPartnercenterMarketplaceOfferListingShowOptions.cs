using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "show")]
public record AzPartnercenterMarketplaceOfferListingShowOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions
{
}