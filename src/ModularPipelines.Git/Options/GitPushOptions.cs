using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("push")]
[ExcludeFromCodeCoverage]
public record GitPushOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--branches")]
    public virtual bool? Branches { get; set; }

    [BooleanCommandSwitch("--prune")]
    public virtual bool? Prune { get; set; }

    [BooleanCommandSwitch("--mirror")]
    public virtual bool? Mirror { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--delete")]
    public virtual bool? Delete { get; set; }

    [BooleanCommandSwitch("--tags")]
    public virtual bool? Tags { get; set; }

    [BooleanCommandSwitch("--follow-tags")]
    public virtual bool? FollowTags { get; set; }

    [BooleanCommandSwitch("--no-signed")]
    public virtual bool? NoSigned { get; set; }

    [BooleanCommandSwitch("--signed")]
    public virtual bool? Signed { get; set; }

    [BooleanCommandSwitch("--no-atomic")]
    public virtual bool? NoAtomic { get; set; }

    [BooleanCommandSwitch("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CommandEqualsSeparatorSwitch("--push-option")]
    public string? PushOption { get; set; }

    [CommandEqualsSeparatorSwitch("--receive-pack")]
    public string? ReceivePack { get; set; }

    [CommandEqualsSeparatorSwitch("--exec")]
    public string? Exec { get; set; }

    [BooleanCommandSwitch("--no-force-with-lease")]
    public virtual bool? NoForceWithLease { get; set; }

    [BooleanCommandSwitch("--force-with-lease")]
    public virtual bool? ForceWithLease { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--no-force-if-includes")]
    public virtual bool? NoForceIfIncludes { get; set; }

    [BooleanCommandSwitch("--force-if-includes")]
    public virtual bool? ForceIfIncludes { get; set; }

    [CommandEqualsSeparatorSwitch("--repo")]
    public string? Repo { get; set; }

    [BooleanCommandSwitch("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [BooleanCommandSwitch("--no-thin")]
    public virtual bool? NoThin { get; set; }

    [BooleanCommandSwitch("--thin")]
    public virtual bool? Thin { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }

    [BooleanCommandSwitch("--ipv4")]
    public virtual bool? Ipv4 { get; set; }

    [BooleanCommandSwitch("--ipv6")]
    public virtual bool? Ipv6 { get; set; }
}