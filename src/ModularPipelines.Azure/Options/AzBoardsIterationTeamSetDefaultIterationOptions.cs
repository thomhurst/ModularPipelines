using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards", "iteration", "team", "set-default-iteration")]
public record AzBoardsIterationTeamSetDefaultIterationOptions(
[property: CommandSwitch("--team")] string Team
) : AzOptions
{
    [CommandSwitch("--default-iteration-macro")]
    public string? DefaultIterationMacro { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}