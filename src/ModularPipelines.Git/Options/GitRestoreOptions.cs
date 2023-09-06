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
    public bool? Patch { get; set; }

    [BooleanCommandSwitch("--worktree")]
    public bool? Worktree { get; set; }

    [BooleanCommandSwitch("--staged")]
    public bool? Staged { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--ours")]
    public bool? Ours { get; set; }

    [BooleanCommandSwitch("--theirs")]
    public bool? Theirs { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--conflict")]
    public string? Conflict { get; set; }

    [BooleanCommandSwitch("--ignore-unmerged")]
    public bool? IgnoreUnmerged { get; set; }

    [BooleanCommandSwitch("--ignore-skip-worktree-bits")]
    public bool? IgnoreSkipWorktreeBits { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--overlay")]
    public bool? Overlay { get; set; }

    [BooleanCommandSwitch("--no-overlay")]
    public bool? NoOverlay { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public bool? PathspecFileNul { get; set; }
}
