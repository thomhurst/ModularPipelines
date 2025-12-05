using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("self-help", "diagnostic", "show")]
public record AzSelfHelpDiagnosticShowOptions(
[property: CliOption("--diagnostic-name")] string DiagnosticName,
[property: CliOption("--scope")] string Scope
) : AzOptions;