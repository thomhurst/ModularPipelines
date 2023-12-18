using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "troubleshooter", "create")]
public record AzSelfHelpTroubleshooterCreateOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--troubleshooter-name")] string TroubleshooterName
) : AzOptions
{
    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--solution-id")]
    public string? SolutionId { get; set; }
}