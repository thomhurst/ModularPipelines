using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("self-help", "solution", "show")]
public record AzSelfHelpSolutionShowOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--solution-name")] string SolutionName
) : AzOptions;