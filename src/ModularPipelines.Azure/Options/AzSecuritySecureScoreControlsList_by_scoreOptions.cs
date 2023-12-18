using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "secure-score-controls", "list_by_score")]
public record AzSecuritySecureScoreControlsList_by_scoreOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;