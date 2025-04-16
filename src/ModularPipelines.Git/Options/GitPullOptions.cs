using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("pull")]
[ExcludeFromCodeCoverage]
public record GitPullOptions : GitOptions
{
    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--commit")]
    public virtual bool? Commit { get; set; }

    [BooleanCommandSwitch("--no-commit")]
    public virtual bool? NoCommit { get; set; }

    [BooleanCommandSwitch("--edit")]
    public virtual bool? Edit { get; set; }

    [BooleanCommandSwitch("--no-edit")]
    public virtual bool? NoEdit { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("--ff-only")]
    public virtual bool? FfOnly { get; set; }

    [BooleanCommandSwitch("--ff")]
    public virtual bool? Ff { get; set; }

    [BooleanCommandSwitch("--no-ff")]
    public virtual bool? NoFf { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [CommandEqualsSeparatorSwitch("--log")]
    public string? Log { get; set; }

    [BooleanCommandSwitch("--no-log")]
    public virtual bool? NoLog { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public virtual bool? Signoff { get; set; }

    [BooleanCommandSwitch("--no-signoff")]
    public virtual bool? NoSignoff { get; set; }

    [BooleanCommandSwitch("--stat")]
    public virtual bool? Stat { get; set; }

    [BooleanCommandSwitch("--no-stat")]
    public virtual bool? NoStat { get; set; }

    [BooleanCommandSwitch("--squash")]
    public virtual bool? Squash { get; set; }

    [BooleanCommandSwitch("--no-squash")]
    public virtual bool? NoSquash { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("--verify-signatures")]
    public virtual bool? VerifySignatures { get; set; }

    [BooleanCommandSwitch("--no-verify-signatures")]
    public virtual bool? NoVerifySignatures { get; set; }

    [BooleanCommandSwitch("--summary")]
    public virtual bool? Summary { get; set; }

    [BooleanCommandSwitch("--no-summary")]
    public virtual bool? NoSummary { get; set; }

    [BooleanCommandSwitch("--autostash")]
    public virtual bool? Autostash { get; set; }

    [BooleanCommandSwitch("--no-autostash")]
    public virtual bool? NoAutostash { get; set; }

    [BooleanCommandSwitch("--allow-unrelated-histories")]
    public virtual bool? AllowUnrelatedHistories { get; set; }

    [BooleanCommandSwitch("--rebase")]
    public virtual bool? Rebase { get; set; }

    [BooleanCommandSwitch("--no-rebase")]
    public virtual bool? NoRebase { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--append")]
    public virtual bool? Append { get; set; }

    [BooleanCommandSwitch("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CommandEqualsSeparatorSwitch("--depth")]
    public string? Depth { get; set; }

    [CommandEqualsSeparatorSwitch("--deepen")]
    public string? Deepen { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-since")]
    public string? ShallowSince { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-exclude")]
    public string? ShallowExclude { get; set; }

    [BooleanCommandSwitch("--unshallow")]
    public virtual bool? Unshallow { get; set; }

    [BooleanCommandSwitch("--update-shallow")]
    public virtual bool? UpdateShallow { get; set; }

    [CommandEqualsSeparatorSwitch("--negotiation-tip")]
    public string? NegotiationTip { get; set; }

    [BooleanCommandSwitch("--negotiate-only")]
    public virtual bool? NegotiateOnly { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--keep")]
    public virtual bool? Keep { get; set; }

    [BooleanCommandSwitch("--prefetch")]
    public virtual bool? Prefetch { get; set; }

    [BooleanCommandSwitch("--prune")]
    public virtual bool? Prune { get; set; }

    [BooleanCommandSwitch("--no-tags")]
    public virtual bool? NoTags { get; set; }

    [CommandEqualsSeparatorSwitch("--refmap")]
    public string? Refmap { get; set; }

    [BooleanCommandSwitch("--tags")]
    public virtual bool? Tags { get; set; }

    [CommandEqualsSeparatorSwitch("--jobs")]
    public string? Jobs { get; set; }

    [BooleanCommandSwitch("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [CommandEqualsSeparatorSwitch("--upload-pack")]
    public string? UploadPack { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--server-option")]
    public string? ServerOption { get; set; }

    [BooleanCommandSwitch("--show-forced-updates")]
    public virtual bool? ShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("--no-show-forced-updates")]
    public virtual bool? NoShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("--ipv4")]
    public virtual bool? Ipv4 { get; set; }

    [BooleanCommandSwitch("--ipv6")]
    public virtual bool? Ipv6 { get; set; }
}