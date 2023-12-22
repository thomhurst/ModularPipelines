using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "submission", "list")]
public record AzPartnercenterMarketplaceOfferSubmissionListOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions;