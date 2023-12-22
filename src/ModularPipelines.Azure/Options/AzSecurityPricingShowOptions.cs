using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "pricing", "show")]
public record AzSecurityPricingShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;