using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "external-security-solution", "list")]
public record AzSecurityExternalSecuritySolutionListOptions : AzOptions;