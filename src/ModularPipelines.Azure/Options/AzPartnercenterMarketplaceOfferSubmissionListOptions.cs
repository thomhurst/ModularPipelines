using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "submission", "list")]
public record AzPartnercenterMarketplaceOfferSubmissionListOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions;