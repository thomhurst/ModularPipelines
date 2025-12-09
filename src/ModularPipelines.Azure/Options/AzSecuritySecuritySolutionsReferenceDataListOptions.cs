using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "security-solutions-reference-data", "list")]
public record AzSecuritySecuritySolutionsReferenceDataListOptions : AzOptions;