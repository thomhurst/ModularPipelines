using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("self-help", "diagnostic", "create")]
public record AzSelfHelpDiagnosticCreateOptions(
[property: CliOption("--diagnostic-name")] string DiagnosticName,
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--global-parameters")]
    public string? GlobalParameters { get; set; }

    [CliOption("--insights")]
    public string? Insights { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}