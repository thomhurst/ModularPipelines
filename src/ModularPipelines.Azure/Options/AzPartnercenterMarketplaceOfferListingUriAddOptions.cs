using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "uri", "add")]
public record AzPartnercenterMarketplaceOfferListingUriAddOptions(
[property: CommandSwitch("--display-text")] string DisplayText,
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--subtype")] string Subtype,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}