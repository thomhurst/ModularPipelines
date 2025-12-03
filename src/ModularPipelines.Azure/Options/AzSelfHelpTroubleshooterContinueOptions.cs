using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("self-help", "troubleshooter", "continue")]
public record AzSelfHelpTroubleshooterContinueOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--troubleshooter-name")] string TroubleshooterName
) : AzOptions
{
    [CliOption("--responses")]
    public string? Responses { get; set; }

    [CliOption("--step-id")]
    public string? StepId { get; set; }
}