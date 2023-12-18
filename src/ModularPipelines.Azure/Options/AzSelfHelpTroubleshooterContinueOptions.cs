using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "troubleshooter", "continue")]
public record AzSelfHelpTroubleshooterContinueOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--troubleshooter-name")] string TroubleshooterName
) : AzOptions
{
    [CommandSwitch("--responses")]
    public string? Responses { get; set; }

    [CommandSwitch("--step-id")]
    public string? StepId { get; set; }
}