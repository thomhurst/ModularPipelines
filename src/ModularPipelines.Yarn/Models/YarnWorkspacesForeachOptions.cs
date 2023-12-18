using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "foreach")]
public record YarnWorkspacesForeachOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string CommandName
) : YarnOptions
{
    [CommandSwitch("--from")]
    public string? From { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--worktree")]
    public bool? Worktree { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--parallel")]
    public bool? Parallel { get; set; }

    [BooleanCommandSwitch("--interlaced")]
    public bool? Interlaced { get; set; }

    [CommandSwitch("--jobs")]
    public string? Jobs { get; set; }

    [BooleanCommandSwitch("--topological")]
    public bool? Topological { get; set; }

    [BooleanCommandSwitch("--topological-dev")]
    public bool? TopologicalDev { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [CommandSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--no-private")]
    public bool? NoPrivate { get; set; }

    [BooleanCommandSwitch("--since")]
    public bool? Since { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}