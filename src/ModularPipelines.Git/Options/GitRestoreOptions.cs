using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("restore")]
[ExcludeFromCodeCoverage]
public record GitRestoreOptions : GitOptions
{
    [CliOption("--source", Format = OptionFormat.EqualsSeparated)]
    public string? Source { get; set; }

    [CliFlag("--patch")]
    public virtual bool? Patch { get; set; }

    [CliFlag("--worktree")]
    public virtual bool? Worktree { get; set; }

    [CliFlag("--staged")]
    public virtual bool? Staged { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CliFlag("--ours")]
    public virtual bool? Ours { get; set; }

    [CliFlag("--theirs")]
    public virtual bool? Theirs { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliOption("--conflict", Format = OptionFormat.EqualsSeparated)]
    public string? Conflict { get; set; }

    [CliFlag("--ignore-unmerged")]
    public virtual bool? IgnoreUnmerged { get; set; }

    [CliFlag("--ignore-skip-worktree-bits")]
    public virtual bool? IgnoreSkipWorktreeBits { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [CliFlag("--overlay")]
    public virtual bool? Overlay { get; set; }

    [CliFlag("--no-overlay")]
    public virtual bool? NoOverlay { get; set; }

    [CliOption("--pathspec-from-file", Format = OptionFormat.EqualsSeparated)]
    public string? PathspecFromFile { get; set; }

    [CliFlag("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }
}