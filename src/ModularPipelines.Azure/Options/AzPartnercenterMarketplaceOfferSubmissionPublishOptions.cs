using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter", "marketplace", "offer", "submission", "publish")]
public record AzPartnercenterMarketplaceOfferSubmissionPublishOptions(
[property: CliOption("--offer-id")] string OfferId,
[property: CliOption("--submission-id")] string SubmissionId,
[property: CliOption("--target")] string Target
) : AzOptions;