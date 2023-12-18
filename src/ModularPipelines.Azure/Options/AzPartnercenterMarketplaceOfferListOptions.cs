using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "list")]
public record AzPartnercenterMarketplaceOfferListOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--target")] string Target
) : AzOptions
{
}

