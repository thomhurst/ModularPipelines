using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "listing", "media", "delete")]
public record AzPartnercenterMarketplaceOfferListingMediaDeleteOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}