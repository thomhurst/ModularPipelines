using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "listing", "media", "add")]
public record AzPartnercenterMarketplaceOfferListingMediaAddOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--id")] string Id,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--streaming-uri")]
    public string? StreamingUri { get; set; }
}