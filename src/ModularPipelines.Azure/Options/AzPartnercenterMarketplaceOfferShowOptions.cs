using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "show")]
public record AzPartnercenterMarketplaceOfferShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
}