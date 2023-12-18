using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "publish")]
public record AzPartnercenterMarketplaceOfferPublishOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--target")] string Target
) : AzOptions
{
}

