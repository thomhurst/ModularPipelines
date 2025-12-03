using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("self-help", "troubleshooter", "end")]
public record AzSelfHelpTroubleshooterEndOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--troubleshooter-name")] string TroubleshooterName
) : AzOptions;