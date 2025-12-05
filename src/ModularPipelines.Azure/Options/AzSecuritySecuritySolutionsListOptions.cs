using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "security-solutions", "list")]
public record AzSecuritySecuritySolutionsListOptions : AzOptions;