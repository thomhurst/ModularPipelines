using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("restore")]
[ExcludeFromCodeCoverage]
public record GitRestoreOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--patch")]
    public virtual bool? Patch { get; set; }

    [BooleanCommandSwitch("--worktree")]
    public virtual bool? Worktree { get; set; }

    [BooleanCommandSwitch("--staged")]
    public virtual bool? Staged { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--ours")]
    public virtual bool? Ours { get; set; }

    [BooleanCommandSwitch("--theirs")]
    public virtual bool? Theirs { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--conflict")]
    public string? Conflict { get; set; }

    [BooleanCommandSwitch("--ignore-unmerged")]
    public virtual bool? IgnoreUnmerged { get; set; }

    [BooleanCommandSwitch("--ignore-skip-worktree-bits")]
    public virtual bool? IgnoreSkipWorktreeBits { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--overlay")]
    public virtual bool? Overlay { get; set; }

    [BooleanCommandSwitch("--no-overlay")]
    public virtual bool? NoOverlay { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }
}