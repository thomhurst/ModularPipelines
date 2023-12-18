using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "show")]
public record AzPartnercenterMarketplaceOfferShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;