using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "contact", "delete")]
public record AzPartnercenterMarketplaceOfferListingContactDeleteOptions(
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--phone")]
    public string? Phone { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}