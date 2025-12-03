using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "secure-score-controls", "list_by_score")]
public record AzSecuritySecureScoreControlsList_by_scoreOptions(
[property: CliOption("--name")] string Name
) : AzOptions;