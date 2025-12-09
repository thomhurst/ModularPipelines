using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "discovered-security-solution", "list")]
public record AzSecurityDiscoveredSecuritySolutionListOptions : AzOptions;