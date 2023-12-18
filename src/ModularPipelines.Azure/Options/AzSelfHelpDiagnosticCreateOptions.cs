using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "diagnostic", "create")]
public record AzSelfHelpDiagnosticCreateOptions(
[property: CommandSwitch("--diagnostic-name")] string DiagnosticName,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--global-parameters")]
    public string? GlobalParameters { get; set; }

    [CommandSwitch("--insights")]
    public string? Insights { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}