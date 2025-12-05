using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "create")]
public record AzPartnercenterMarketplaceOfferCreateOptions(
[property: CliOption("--alias")] string Alias,
[property: CliOption("--id")] string Id,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}