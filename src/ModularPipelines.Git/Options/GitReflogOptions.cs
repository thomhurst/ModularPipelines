using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("reflog")]
[ExcludeFromCodeCoverage]
public record GitReflogOptions : GitOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--single-worktree")]
    public virtual bool? SingleWorktree { get; set; }

    [CliOption("--expire", Format = OptionFormat.EqualsSeparated)]
    public string? Expire { get; set; }

    [CliOption("--expire-unreachable", Format = OptionFormat.EqualsSeparated)]
    public string? ExpireUnreachable { get; set; }

    [CliFlag("--updateref")]
    public virtual bool? Updateref { get; set; }

    [CliFlag("--rewrite")]
    public virtual bool? Rewrite { get; set; }

    [CliFlag("--stale-fix")]
    public virtual bool? StaleFix { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }
}