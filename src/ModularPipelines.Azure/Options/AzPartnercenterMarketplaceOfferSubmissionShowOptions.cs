using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "submission", "show")]
public record AzPartnercenterMarketplaceOfferSubmissionShowOptions(
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--submission-id")] string SubmissionId
) : AzOptions;