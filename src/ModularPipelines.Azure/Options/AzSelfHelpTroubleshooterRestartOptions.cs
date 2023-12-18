using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "troubleshooter", "restart")]
public record AzSelfHelpTroubleshooterRestartOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--troubleshooter-name")] string TroubleshooterName
) : AzOptions
{
}

