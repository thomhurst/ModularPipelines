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
    public virtual string? From { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [BooleanCommandSwitch("--worktree")]
    public virtual bool? Worktree { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--parallel")]
    public virtual bool? Parallel { get; set; }

    [BooleanCommandSwitch("--interlaced")]
    public virtual bool? Interlaced { get; set; }

    [CommandSwitch("--jobs")]
    public virtual string? Jobs { get; set; }

    [BooleanCommandSwitch("--topological")]
    public virtual bool? Topological { get; set; }

    [BooleanCommandSwitch("--topological-dev")]
    public virtual bool? TopologicalDev { get; set; }

    [CommandSwitch("--include")]
    public virtual string? Include { get; set; }

    [CommandSwitch("--exclude")]
    public virtual string? Exclude { get; set; }

    [BooleanCommandSwitch("--no-private")]
    public virtual bool? NoPrivate { get; set; }

    [BooleanCommandSwitch("--since")]
    public virtual bool? Since { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }
}