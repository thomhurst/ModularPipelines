using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "discovered-security-solution", "show")]
public record AzSecurityDiscoveredSecuritySolutionShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;