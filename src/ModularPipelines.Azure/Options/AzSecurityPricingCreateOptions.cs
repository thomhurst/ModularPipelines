using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "pricing", "create")]
public record AzSecurityPricingCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--tier")] string Tier
) : AzOptions
{
    [CommandSwitch("--extensions")]
    public string? Extensions { get; set; }

    [CommandSwitch("--subplan")]
    public string? Subplan { get; set; }
}