using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "troubleshooter", "end")]
public record AzSelfHelpTroubleshooterEndOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--troubleshooter-name")] string TroubleshooterName
) : AzOptions
{
}