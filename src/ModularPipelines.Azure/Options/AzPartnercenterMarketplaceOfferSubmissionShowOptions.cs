using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "submission", "show")]
public record AzPartnercenterMarketplaceOfferSubmissionShowOptions(
[property: CommandSwitch("--offer-id")] string OfferId,
[property: CommandSwitch("--submission-id")] string SubmissionId
) : AzOptions
{
}