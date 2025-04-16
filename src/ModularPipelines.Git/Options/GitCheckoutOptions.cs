using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("checkout")]
[ExcludeFromCodeCoverage]
public record GitCheckoutOptions : GitOptions
{
    public GitCheckoutOptions(string branchName) : this(branchName, false)
    {
    }

    public GitCheckoutOptions(string branchName, bool create)
    {
        if (create)
        {
            NewBranchName = branchName;
        }
        else
        {
            BranchName = branchName;
        }
    }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--ours")]
    public virtual bool? Ours { get; set; }

    [BooleanCommandSwitch("--theirs")]
    public virtual bool? Theirs { get; set; }

    [BooleanCommandSwitch("--track")]
    public virtual bool? Track { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [BooleanCommandSwitch("--guess")]
    public virtual bool? Guess { get; set; }

    [BooleanCommandSwitch("--no-guess")]
    public virtual bool? NoGuess { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--orphan")]
    public virtual string? Orphan { get; set; }

    [BooleanCommandSwitch("--ignore-skip-worktree-bits")]
    public virtual bool? IgnoreSkipWorktreeBits { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--conflict")]
    public string? Conflict { get; set; }

    [BooleanCommandSwitch("--patch")]
    public virtual bool? Patch { get; set; }

    [BooleanCommandSwitch("--ignore-other-worktrees")]
    public virtual bool? IgnoreOtherWorktrees { get; set; }

    [BooleanCommandSwitch("--overwrite-ignore")]
    public virtual bool? OverwriteIgnore { get; set; }

    [BooleanCommandSwitch("--no-overwrite-ignore")]
    public virtual bool? NoOverwriteIgnore { get; set; }

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

    [PositionalArgument]
    public string? BranchName { get; set; }

    [CommandSwitch("-b")]
    public virtual string? NewBranchName { get; set; }
}