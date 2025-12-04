using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "pricing", "show")]
public record AzSecurityPricingShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;