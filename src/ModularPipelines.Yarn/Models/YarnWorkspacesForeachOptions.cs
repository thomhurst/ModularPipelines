using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "foreach")]
public record YarnWorkspacesForeachOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string CommandName
) : YarnOptions
{
    [CliOption("--from")]
    public virtual string? From { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--worktree")]
    public virtual bool? Worktree { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--parallel")]
    public virtual bool? Parallel { get; set; }

    [CliFlag("--interlaced")]
    public virtual bool? Interlaced { get; set; }

    [CliOption("--jobs")]
    public virtual string? Jobs { get; set; }

    [CliFlag("--topological")]
    public virtual bool? Topological { get; set; }

    [CliFlag("--topological-dev")]
    public virtual bool? TopologicalDev { get; set; }

    [CliOption("--include")]
    public virtual string? Include { get; set; }

    [CliOption("--exclude")]
    public virtual string? Exclude { get; set; }

    [CliFlag("--no-private")]
    public virtual bool? NoPrivate { get; set; }

    [CliFlag("--since")]
    public virtual bool? Since { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }
}