using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "plan", "delete")]
public record AzPartnercenterMarketplaceOfferPlanDeleteOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions
{
}