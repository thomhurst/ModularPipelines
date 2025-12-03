using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "listing", "uri", "delete")]
public record AzPartnercenterMarketplaceOfferListingUriDeleteOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions
{
    [CliOption("--display-text")]
    public string? DisplayText { get; set; }

    [CliOption("--subtype")]
    public string? Subtype { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}