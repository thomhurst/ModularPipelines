using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "plan", "delete")]
public record AzPartnercenterMarketplaceOfferPlanDeleteOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;