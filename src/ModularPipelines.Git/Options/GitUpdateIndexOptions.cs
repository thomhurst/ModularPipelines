using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("update-index")]
[ExcludeFromCodeCoverage]
public record GitUpdateIndexOptions : GitOptions
{
    [BooleanCommandSwitch("--add")]
    public bool? Add { get; set; }

    [BooleanCommandSwitch("--remove")]
    public bool? Remove { get; set; }

    [BooleanCommandSwitch("--refresh")]
    public bool? Refresh { get; set; }

    [BooleanCommandSwitch("--ignore-submodules")]
    public bool? IgnoreSubmodules { get; set; }

    [BooleanCommandSwitch("--unmerged")]
    public bool? Unmerged { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public bool? IgnoreMissing { get; set; }

    [CommandEqualsSeparatorSwitch("--cacheinfo")]
    public string? Cacheinfo { get; set; }

    [BooleanCommandSwitch("--index-info")]
    public bool? IndexInfo { get; set; }

    [BooleanCommandSwitch("--chmod")]
    public bool? Chmod { get; set; }

    [BooleanCommandSwitch("--no-assume-unchanged")]
    public bool? NoAssumeUnchanged { get; set; }

    [BooleanCommandSwitch("--assume-unchanged")]
    public bool? AssumeUnchanged { get; set; }

    [BooleanCommandSwitch("--really-refresh")]
    public bool? ReallyRefresh { get; set; }

    [BooleanCommandSwitch("--no-skip-worktree")]
    public bool? NoSkipWorktree { get; set; }

    [BooleanCommandSwitch("--skip-worktree")]
    public bool? SkipWorktree { get; set; }

    [BooleanCommandSwitch("--no-ignore-skip-worktree-entries")]
    public bool? NoIgnoreSkipWorktreeEntries { get; set; }

    [BooleanCommandSwitch("--ignore-skip-worktree-entries")]
    public bool? IgnoreSkipWorktreeEntries { get; set; }

    [BooleanCommandSwitch("--no-fsmonitor-valid")]
    public bool? NoFsmonitorValid { get; set; }

    [BooleanCommandSwitch("--fsmonitor-valid")]
    public bool? FsmonitorValid { get; set; }

    [BooleanCommandSwitch("--again")]
    public bool? Again { get; set; }

    [BooleanCommandSwitch("--unresolve")]
    public bool? Unresolve { get; set; }

    [BooleanCommandSwitch("--info-only")]
    public bool? InfoOnly { get; set; }

    [BooleanCommandSwitch("--force-remove")]
    public bool? ForceRemove { get; set; }

    [BooleanCommandSwitch("--replace")]
    public bool? Replace { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--index-version")]
    public string? IndexVersion { get; set; }

    [BooleanCommandSwitch("--split-index")]
    public bool? SplitIndex { get; set; }

    [BooleanCommandSwitch("--no-split-index")]
    public bool? NoSplitIndex { get; set; }

    [BooleanCommandSwitch("--untracked-cache")]
    public bool? UntrackedCache { get; set; }

    [BooleanCommandSwitch("--no-untracked-cache")]
    public bool? NoUntrackedCache { get; set; }

    [BooleanCommandSwitch("--test-untracked-cache")]
    public bool? TestUntrackedCache { get; set; }

    [BooleanCommandSwitch("--force-untracked-cache")]
    public bool? ForceUntrackedCache { get; set; }

    [BooleanCommandSwitch("--fsmonitor")]
    public bool? Fsmonitor { get; set; }

    [BooleanCommandSwitch("--no-fsmonitor")]
    public bool? NoFsmonitor { get; set; }
}