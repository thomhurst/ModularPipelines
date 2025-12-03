using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "listing", "media", "delete")]
public record AzPartnercenterMarketplaceOfferListingMediaDeleteOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}