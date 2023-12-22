using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "publish")]
public record AzPartnercenterMarketplaceOfferPublishOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--target")] string Target
) : AzOptions;