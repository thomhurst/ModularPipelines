using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards", "iteration", "team", "set-backlog-iteration")]
public record AzBoardsIterationTeamSetBacklogIterationOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--team")] string Team
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}