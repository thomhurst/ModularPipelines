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
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--ours")]
    public bool? Ours { get; set; }

    [BooleanCommandSwitch("--theirs")]
    public bool? Theirs { get; set; }

    [BooleanCommandSwitch("--track")]
    public bool? Track { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public bool? NoTrack { get; set; }

    [BooleanCommandSwitch("--guess")]
    public bool? Guess { get; set; }

    [BooleanCommandSwitch("--no-guess")]
    public bool? NoGuess { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--orphan")]
    public string? Orphan { get; set; }

    [BooleanCommandSwitch("--ignore-skip-worktree-bits")]
    public bool? IgnoreSkipWorktreeBits { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--conflict")]
    public string? Conflict { get; set; }

    [BooleanCommandSwitch("--patch")]
    public bool? Patch { get; set; }

    [BooleanCommandSwitch("--ignore-other-worktrees")]
    public bool? IgnoreOtherWorktrees { get; set; }

    [BooleanCommandSwitch("--overwrite-ignore")]
    public bool? OverwriteIgnore { get; set; }

    [BooleanCommandSwitch("--no-overwrite-ignore")]
    public bool? NoOverwriteIgnore { get; set; }

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

    [PositionalArgument] 
    public string? BranchName { get; set; }
    
    [CommandSwitch("-b")] 
    public string? NewBranchName { get; set; }
}