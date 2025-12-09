using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("boards", "iteration", "team", "set-backlog-iteration")]
public record AzBoardsIterationTeamSetBacklogIterationOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--team")] string Team
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}