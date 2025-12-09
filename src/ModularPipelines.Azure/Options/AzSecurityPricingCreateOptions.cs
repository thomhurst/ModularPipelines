using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "pricing", "create")]
public record AzSecurityPricingCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--tier")] string Tier
) : AzOptions
{
    [CliOption("--extensions")]
    public string? Extensions { get; set; }

    [CliOption("--subplan")]
    public string? Subplan { get; set; }
}