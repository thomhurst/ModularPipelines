using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "submission", "publish")]
public record AzPartnercenterMarketplaceOfferSubmissionPublishOptions(
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--submission-id")] string SubmissionId,
[property: CommandSwitch("--target")] string Target
) : AzOptions;