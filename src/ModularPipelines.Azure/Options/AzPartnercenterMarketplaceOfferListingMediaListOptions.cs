using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "listing", "media", "list")]
public record AzPartnercenterMarketplaceOfferListingMediaListOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--type")] string Type
) : AzOptions;