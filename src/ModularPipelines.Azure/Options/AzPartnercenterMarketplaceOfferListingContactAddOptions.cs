using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "listing", "contact", "add")]
public record AzPartnercenterMarketplaceOfferListingContactAddOptions(
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }
}