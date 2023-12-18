using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "diagnostic", "show")]
public record AzSelfHelpDiagnosticShowOptions(
[property: CommandSwitch("--diagnostic-name")] string DiagnosticName,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
}