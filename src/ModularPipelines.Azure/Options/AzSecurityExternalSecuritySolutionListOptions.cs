using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "external-security-solution", "list")]
public record AzSecurityExternalSecuritySolutionListOptions : AzOptions;