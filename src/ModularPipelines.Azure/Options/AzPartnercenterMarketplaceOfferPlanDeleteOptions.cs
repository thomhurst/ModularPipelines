using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "plan", "delete")]
public record AzPartnercenterMarketplaceOfferPlanDeleteOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;