using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("reflog")]
public record GitReflogOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--single-worktree")]
    public bool? SingleWorktree { get; set; }

    [CommandEqualsSeparatorSwitch("--expire")]
    public string? Expire { get; set; }

    [CommandEqualsSeparatorSwitch("--expire-unreachable")]
    public string? ExpireUnreachable { get; set; }

    [BooleanCommandSwitch("--updateref")]
    public bool? Updateref { get; set; }

    [BooleanCommandSwitch("--rewrite")]
    public bool? Rewrite { get; set; }

    [BooleanCommandSwitch("--stale-fix")]
    public bool? StaleFix { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

}