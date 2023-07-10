using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("push")]
public record GitPushOptions : GitOptions
{
    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("branches")]
    public bool? Branches { get; set; }

    [BooleanCommandSwitch("prune")]
    public bool? Prune { get; set; }

    [BooleanCommandSwitch("mirror")]
    public bool? Mirror { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("porcelain")]
    public bool? Porcelain { get; set; }

    [BooleanCommandSwitch("delete")]
    public bool? Delete { get; set; }

    [BooleanCommandSwitch("tags")]
    public bool? Tags { get; set; }

    [BooleanCommandSwitch("follow-tags")]
    public bool? FollowTags { get; set; }

    [BooleanCommandSwitch("no-signed")]
    public bool? NoSigned { get; set; }

    [BooleanCommandSwitch("signed")]
    public bool? Signed { get; set; }

    [BooleanCommandSwitch("no-atomic")]
    public bool? NoAtomic { get; set; }

    [BooleanCommandSwitch("atomic")]
    public bool? Atomic { get; set; }

    [CommandLongSwitch("push-option")]
    public string? PushOption { get; set; }

    [CommandLongSwitch("receive-pack")]
    public string? ReceivePack { get; set; }

    [CommandLongSwitch("exec")]
    public string? Exec { get; set; }

    [BooleanCommandSwitch("no-force-with-lease")]
    public bool? NoForceWithLease { get; set; }

    [BooleanCommandSwitch("force-with-lease")]
    public bool? ForceWithLease { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("no-force-if-includes")]
    public bool? NoForceIfIncludes { get; set; }

    [BooleanCommandSwitch("force-if-includes")]
    public bool? ForceIfIncludes { get; set; }

    [CommandLongSwitch("repo")]
    public string? Repo { get; set; }

    [BooleanCommandSwitch("set-upstream")]
    public bool? SetUpstream { get; set; }

    [BooleanCommandSwitch("no-thin")]
    public bool? NoThin { get; set; }

    [BooleanCommandSwitch("thin")]
    public bool? Thin { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("progress")]
    public bool? Progress { get; set; }

    [BooleanCommandSwitch("no-recurse-submodules")]
    public bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("no-verify")]
    public bool? NoVerify { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

    [BooleanCommandSwitch("ipv4")]
    public bool? Ipv4 { get; set; }

    [BooleanCommandSwitch("ipv6")]
    public bool? Ipv6 { get; set; }

}