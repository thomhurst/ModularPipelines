using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "uri", "delete")]
public record AzPartnercenterMarketplaceOfferListingUriDeleteOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions
{
    [CommandSwitch("--display-text")]
    public string? DisplayText { get; set; }

    [CommandSwitch("--subtype")]
    public string? Subtype { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}