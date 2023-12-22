using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "setup", "show")]
public record AzPartnercenterMarketplaceOfferSetupShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;