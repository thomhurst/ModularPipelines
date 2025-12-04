using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("checkout")]
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

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--ours")]
    public virtual bool? Ours { get; set; }

    [CliFlag("--theirs")]
    public virtual bool? Theirs { get; set; }

    [CliFlag("--track")]
    public virtual bool? Track { get; set; }

    [CliFlag("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [CliFlag("--guess")]
    public virtual bool? Guess { get; set; }

    [CliFlag("--no-guess")]
    public virtual bool? NoGuess { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--orphan")]
    public virtual string? Orphan { get; set; }

    [CliFlag("--ignore-skip-worktree-bits")]
    public virtual bool? IgnoreSkipWorktreeBits { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliOption("--conflict", Format = OptionFormat.EqualsSeparated)]
    public string? Conflict { get; set; }

    [CliFlag("--patch")]
    public virtual bool? Patch { get; set; }

    [CliFlag("--ignore-other-worktrees")]
    public virtual bool? IgnoreOtherWorktrees { get; set; }

    [CliFlag("--overwrite-ignore")]
    public virtual bool? OverwriteIgnore { get; set; }

    [CliFlag("--no-overwrite-ignore")]
    public virtual bool? NoOverwriteIgnore { get; set; }

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

    [CliArgument]
    public string? BranchName { get; set; }

    [CliOption("-b")]
    public virtual string? NewBranchName { get; set; }
}