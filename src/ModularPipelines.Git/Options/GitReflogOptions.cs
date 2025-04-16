using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("reflog")]
[ExcludeFromCodeCoverage]
public record GitReflogOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--single-worktree")]
    public virtual bool? SingleWorktree { get; set; }

    [CommandEqualsSeparatorSwitch("--expire")]
    public string? Expire { get; set; }

    [CommandEqualsSeparatorSwitch("--expire-unreachable")]
    public string? ExpireUnreachable { get; set; }

    [BooleanCommandSwitch("--updateref")]
    public virtual bool? Updateref { get; set; }

    [BooleanCommandSwitch("--rewrite")]
    public virtual bool? Rewrite { get; set; }

    [BooleanCommandSwitch("--stale-fix")]
    public virtual bool? StaleFix { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }
}