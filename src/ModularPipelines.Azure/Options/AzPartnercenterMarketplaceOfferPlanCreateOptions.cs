using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "plan", "create")]
public record AzPartnercenterMarketplaceOfferPlanCreateOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--name")] string Name,
[property: CliOption("--offer-id")] string OfferId
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--subtype")]
    public string? Subtype { get; set; }
}