using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("update-index")]
[ExcludeFromCodeCoverage]
public record GitUpdateIndexOptions : GitOptions
{
    [BooleanCommandSwitch("--add")]
    public virtual bool? Add { get; set; }

    [BooleanCommandSwitch("--remove")]
    public virtual bool? Remove { get; set; }

    [BooleanCommandSwitch("--refresh")]
    public virtual bool? Refresh { get; set; }

    [BooleanCommandSwitch("--ignore-submodules")]
    public virtual bool? IgnoreSubmodules { get; set; }

    [BooleanCommandSwitch("--unmerged")]
    public virtual bool? Unmerged { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [CommandEqualsSeparatorSwitch("--cacheinfo")]
    public string? Cacheinfo { get; set; }

    [BooleanCommandSwitch("--index-info")]
    public virtual bool? IndexInfo { get; set; }

    [BooleanCommandSwitch("--chmod")]
    public virtual bool? Chmod { get; set; }

    [BooleanCommandSwitch("--no-assume-unchanged")]
    public virtual bool? NoAssumeUnchanged { get; set; }

    [BooleanCommandSwitch("--assume-unchanged")]
    public virtual bool? AssumeUnchanged { get; set; }

    [BooleanCommandSwitch("--really-refresh")]
    public virtual bool? ReallyRefresh { get; set; }

    [BooleanCommandSwitch("--no-skip-worktree")]
    public virtual bool? NoSkipWorktree { get; set; }

    [BooleanCommandSwitch("--skip-worktree")]
    public virtual bool? SkipWorktree { get; set; }

    [BooleanCommandSwitch("--no-ignore-skip-worktree-entries")]
    public virtual bool? NoIgnoreSkipWorktreeEntries { get; set; }

    [BooleanCommandSwitch("--ignore-skip-worktree-entries")]
    public virtual bool? IgnoreSkipWorktreeEntries { get; set; }

    [BooleanCommandSwitch("--no-fsmonitor-valid")]
    public virtual bool? NoFsmonitorValid { get; set; }

    [BooleanCommandSwitch("--fsmonitor-valid")]
    public virtual bool? FsmonitorValid { get; set; }

    [BooleanCommandSwitch("--again")]
    public virtual bool? Again { get; set; }

    [BooleanCommandSwitch("--unresolve")]
    public virtual bool? Unresolve { get; set; }

    [BooleanCommandSwitch("--info-only")]
    public virtual bool? InfoOnly { get; set; }

    [BooleanCommandSwitch("--force-remove")]
    public virtual bool? ForceRemove { get; set; }

    [BooleanCommandSwitch("--replace")]
    public virtual bool? Replace { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--index-version")]
    public string? IndexVersion { get; set; }

    [BooleanCommandSwitch("--split-index")]
    public virtual bool? SplitIndex { get; set; }

    [BooleanCommandSwitch("--no-split-index")]
    public virtual bool? NoSplitIndex { get; set; }

    [BooleanCommandSwitch("--untracked-cache")]
    public virtual bool? UntrackedCache { get; set; }

    [BooleanCommandSwitch("--no-untracked-cache")]
    public virtual bool? NoUntrackedCache { get; set; }

    [BooleanCommandSwitch("--test-untracked-cache")]
    public virtual bool? TestUntrackedCache { get; set; }

    [BooleanCommandSwitch("--force-untracked-cache")]
    public virtual bool? ForceUntrackedCache { get; set; }

    [BooleanCommandSwitch("--fsmonitor")]
    public virtual bool? Fsmonitor { get; set; }

    [BooleanCommandSwitch("--no-fsmonitor")]
    public virtual bool? NoFsmonitor { get; set; }
}