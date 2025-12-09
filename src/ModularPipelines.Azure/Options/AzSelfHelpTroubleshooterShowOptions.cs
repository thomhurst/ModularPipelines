using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("self-help", "troubleshooter", "show")]
public record AzSelfHelpTroubleshooterShowOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--troubleshooter-name")] string TroubleshooterName
) : AzOptions;