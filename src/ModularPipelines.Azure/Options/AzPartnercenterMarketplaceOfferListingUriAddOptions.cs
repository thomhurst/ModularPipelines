using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "listing", "uri", "add")]
public record AzPartnercenterMarketplaceOfferListingUriAddOptions(
[property: CliOption("--display-text")] string DisplayText,
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--subtype")] string Subtype,
[property: CliOption("--type")] string Type,
[property: CliOption("--uri")] string Uri
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}