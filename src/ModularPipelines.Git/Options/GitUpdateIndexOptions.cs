using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("update-index")]
[ExcludeFromCodeCoverage]
public record GitUpdateIndexOptions : GitOptions
{
    [CliFlag("--add")]
    public virtual bool? Add { get; set; }

    [CliFlag("--remove")]
    public virtual bool? Remove { get; set; }

    [CliFlag("--refresh")]
    public virtual bool? Refresh { get; set; }

    [CliFlag("--ignore-submodules")]
    public virtual bool? IgnoreSubmodules { get; set; }

    [CliFlag("--unmerged")]
    public virtual bool? Unmerged { get; set; }

    [CliFlag("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [CliOption("--cacheinfo", Format = OptionFormat.EqualsSeparated)]
    public string? Cacheinfo { get; set; }

    [CliFlag("--index-info")]
    public virtual bool? IndexInfo { get; set; }

    [CliFlag("--chmod")]
    public virtual bool? Chmod { get; set; }

    [CliFlag("--no-assume-unchanged")]
    public virtual bool? NoAssumeUnchanged { get; set; }

    [CliFlag("--assume-unchanged")]
    public virtual bool? AssumeUnchanged { get; set; }

    [CliFlag("--really-refresh")]
    public virtual bool? ReallyRefresh { get; set; }

    [CliFlag("--no-skip-worktree")]
    public virtual bool? NoSkipWorktree { get; set; }

    [CliFlag("--skip-worktree")]
    public virtual bool? SkipWorktree { get; set; }

    [CliFlag("--no-ignore-skip-worktree-entries")]
    public virtual bool? NoIgnoreSkipWorktreeEntries { get; set; }

    [CliFlag("--ignore-skip-worktree-entries")]
    public virtual bool? IgnoreSkipWorktreeEntries { get; set; }

    [CliFlag("--no-fsmonitor-valid")]
    public virtual bool? NoFsmonitorValid { get; set; }

    [CliFlag("--fsmonitor-valid")]
    public virtual bool? FsmonitorValid { get; set; }

    [CliFlag("--again")]
    public virtual bool? Again { get; set; }

    [CliFlag("--unresolve")]
    public virtual bool? Unresolve { get; set; }

    [CliFlag("--info-only")]
    public virtual bool? InfoOnly { get; set; }

    [CliFlag("--force-remove")]
    public virtual bool? ForceRemove { get; set; }

    [CliFlag("--replace")]
    public virtual bool? Replace { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliOption("--index-version", Format = OptionFormat.EqualsSeparated)]
    public string? IndexVersion { get; set; }

    [CliFlag("--split-index")]
    public virtual bool? SplitIndex { get; set; }

    [CliFlag("--no-split-index")]
    public virtual bool? NoSplitIndex { get; set; }

    [CliFlag("--untracked-cache")]
    public virtual bool? UntrackedCache { get; set; }

    [CliFlag("--no-untracked-cache")]
    public virtual bool? NoUntrackedCache { get; set; }

    [CliFlag("--test-untracked-cache")]
    public virtual bool? TestUntrackedCache { get; set; }

    [CliFlag("--force-untracked-cache")]
    public virtual bool? ForceUntrackedCache { get; set; }

    [CliFlag("--fsmonitor")]
    public virtual bool? Fsmonitor { get; set; }

    [CliFlag("--no-fsmonitor")]
    public virtual bool? NoFsmonitor { get; set; }
}