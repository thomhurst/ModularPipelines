using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("pull")]
public record GitPullOptions : GitOptions
{
    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("no-recurse-submodules")]
    public bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("commit")]
    public bool? Commit { get; set; }

    [BooleanCommandSwitch("no-commit")]
    public bool? NoCommit { get; set; }

    [BooleanCommandSwitch("edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("no-edit")]
    public bool? NoEdit { get; set; }

    [CommandLongSwitch("cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("ff-only")]
    public bool? FfOnly { get; set; }

    [BooleanCommandSwitch("ff")]
    public bool? Ff { get; set; }

    [BooleanCommandSwitch("no-ff")]
    public bool? NoFf { get; set; }

    [CommandLongSwitch("gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

    [CommandLongSwitch("log")]
    public string? Log { get; set; }

    [BooleanCommandSwitch("no-log")]
    public bool? NoLog { get; set; }

    [BooleanCommandSwitch("signoff")]
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("no-signoff")]
    public bool? NoSignoff { get; set; }

    [BooleanCommandSwitch("stat")]
    public bool? Stat { get; set; }

    [BooleanCommandSwitch("no-stat")]
    public bool? NoStat { get; set; }

    [BooleanCommandSwitch("squash")]
    public bool? Squash { get; set; }

    [BooleanCommandSwitch("no-squash")]
    public bool? NoSquash { get; set; }

    [BooleanCommandSwitch("no-verify")]
    public bool? NoVerify { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

    [CommandLongSwitch("strategy")]
    public string? Strategy { get; set; }

    [CommandLongSwitch("strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("verify-signatures")]
    public bool? VerifySignatures { get; set; }

    [BooleanCommandSwitch("no-verify-signatures")]
    public bool? NoVerifySignatures { get; set; }

    [BooleanCommandSwitch("summary")]
    public bool? Summary { get; set; }

    [BooleanCommandSwitch("no-summary")]
    public bool? NoSummary { get; set; }

    [BooleanCommandSwitch("autostash")]
    public bool? Autostash { get; set; }

    [BooleanCommandSwitch("no-autostash")]
    public bool? NoAutostash { get; set; }

    [BooleanCommandSwitch("allow-unrelated-histories")]
    public bool? AllowUnrelatedHistories { get; set; }

    [BooleanCommandSwitch("rebase")]
    public bool? Rebase { get; set; }

    [BooleanCommandSwitch("no-rebase")]
    public bool? NoRebase { get; set; }

    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("append")]
    public bool? Append { get; set; }

    [BooleanCommandSwitch("atomic")]
    public bool? Atomic { get; set; }

    [CommandLongSwitch("depth")]
    public string? Depth { get; set; }

    [CommandLongSwitch("deepen")]
    public string? Deepen { get; set; }

    [CommandLongSwitch("shallow-since")]
    public string? ShallowSince { get; set; }

    [CommandLongSwitch("shallow-exclude")]
    public string? ShallowExclude { get; set; }

    [BooleanCommandSwitch("unshallow")]
    public bool? Unshallow { get; set; }

    [BooleanCommandSwitch("update-shallow")]
    public bool? UpdateShallow { get; set; }

    [CommandLongSwitch("negotiation-tip")]
    public string? NegotiationTip { get; set; }

    [BooleanCommandSwitch("negotiate-only")]
    public bool? NegotiateOnly { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("porcelain")]
    public bool? Porcelain { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("keep")]
    public bool? Keep { get; set; }

    [BooleanCommandSwitch("prefetch")]
    public bool? Prefetch { get; set; }

    [BooleanCommandSwitch("prune")]
    public bool? Prune { get; set; }

    [BooleanCommandSwitch("no-tags")]
    public bool? NoTags { get; set; }

    [CommandLongSwitch("refmap")]
    public string? Refmap { get; set; }

    [BooleanCommandSwitch("tags")]
    public bool? Tags { get; set; }

    [CommandLongSwitch("jobs")]
    public string? Jobs { get; set; }

    [BooleanCommandSwitch("set-upstream")]
    public bool? SetUpstream { get; set; }

    [CommandLongSwitch("upload-pack")]
    public string? UploadPack { get; set; }

    [BooleanCommandSwitch("progress")]
    public bool? Progress { get; set; }

    [CommandLongSwitch("server-option")]
    public string? ServerOption { get; set; }

    [BooleanCommandSwitch("show-forced-updates")]
    public bool? ShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("no-show-forced-updates")]
    public bool? NoShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("ipv4")]
    public bool? Ipv4 { get; set; }

    [BooleanCommandSwitch("ipv6")]
    public bool? Ipv6 { get; set; }

}