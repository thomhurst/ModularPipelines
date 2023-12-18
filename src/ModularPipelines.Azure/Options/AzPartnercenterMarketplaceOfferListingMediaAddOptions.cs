using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "media", "add")]
public record AzPartnercenterMarketplaceOfferListingMediaAddOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--streaming-uri")]
    public string? StreamingUri { get; set; }
}

