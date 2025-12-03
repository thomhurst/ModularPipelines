using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "listing", "contact", "delete")]
public record AzPartnercenterMarketplaceOfferListingContactDeleteOptions(
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}