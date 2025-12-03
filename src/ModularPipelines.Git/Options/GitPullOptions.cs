using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("pull")]
[ExcludeFromCodeCoverage]
public record GitPullOptions : GitOptions
{
    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--commit")]
    public virtual bool? Commit { get; set; }

    [CliFlag("--no-commit")]
    public virtual bool? NoCommit { get; set; }

    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliFlag("--no-edit")]
    public virtual bool? NoEdit { get; set; }

    [CliOption("--cleanup", Format = OptionFormat.EqualsSeparated)]
    public string? Cleanup { get; set; }

    [CliFlag("--ff-only")]
    public virtual bool? FfOnly { get; set; }

    [CliFlag("--ff")]
    public virtual bool? Ff { get; set; }

    [CliFlag("--no-ff")]
    public virtual bool? NoFf { get; set; }

    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [CliOption("--log", Format = OptionFormat.EqualsSeparated)]
    public string? Log { get; set; }

    [CliFlag("--no-log")]
    public virtual bool? NoLog { get; set; }

    [CliFlag("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CliFlag("--no-signoff")]
    public virtual bool? NoSignoff { get; set; }

    [CliFlag("--stat")]
    public virtual bool? Stat { get; set; }

    [CliFlag("--no-stat")]
    public virtual bool? NoStat { get; set; }

    [CliFlag("--squash")]
    public virtual bool? Squash { get; set; }

    [CliFlag("--no-squash")]
    public virtual bool? NoSquash { get; set; }

    [CliFlag("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliOption("--strategy", Format = OptionFormat.EqualsSeparated)]
    public string? Strategy { get; set; }

    [CliOption("--strategy-option", Format = OptionFormat.EqualsSeparated)]
    public string? StrategyOption { get; set; }

    [CliFlag("--verify-signatures")]
    public virtual bool? VerifySignatures { get; set; }

    [CliFlag("--no-verify-signatures")]
    public virtual bool? NoVerifySignatures { get; set; }

    [CliFlag("--summary")]
    public virtual bool? Summary { get; set; }

    [CliFlag("--no-summary")]
    public virtual bool? NoSummary { get; set; }

    [CliFlag("--autostash")]
    public virtual bool? Autostash { get; set; }

    [CliFlag("--no-autostash")]
    public virtual bool? NoAutostash { get; set; }

    [CliFlag("--allow-unrelated-histories")]
    public virtual bool? AllowUnrelatedHistories { get; set; }

    [CliFlag("--rebase")]
    public virtual bool? Rebase { get; set; }

    [CliFlag("--no-rebase")]
    public virtual bool? NoRebase { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--append")]
    public virtual bool? Append { get; set; }

    [CliFlag("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CliOption("--depth", Format = OptionFormat.EqualsSeparated)]
    public string? Depth { get; set; }

    [CliOption("--deepen", Format = OptionFormat.EqualsSeparated)]
    public string? Deepen { get; set; }

    [CliOption("--shallow-since", Format = OptionFormat.EqualsSeparated)]
    public string? ShallowSince { get; set; }

    [CliOption("--shallow-exclude", Format = OptionFormat.EqualsSeparated)]
    public string? ShallowExclude { get; set; }

    [CliFlag("--unshallow")]
    public virtual bool? Unshallow { get; set; }

    [CliFlag("--update-shallow")]
    public virtual bool? UpdateShallow { get; set; }

    [CliOption("--negotiation-tip", Format = OptionFormat.EqualsSeparated)]
    public string? NegotiationTip { get; set; }

    [CliFlag("--negotiate-only")]
    public virtual bool? NegotiateOnly { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--keep")]
    public virtual bool? Keep { get; set; }

    [CliFlag("--prefetch")]
    public virtual bool? Prefetch { get; set; }

    [CliFlag("--prune")]
    public virtual bool? Prune { get; set; }

    [CliFlag("--no-tags")]
    public virtual bool? NoTags { get; set; }

    [CliOption("--refmap", Format = OptionFormat.EqualsSeparated)]
    public string? Refmap { get; set; }

    [CliFlag("--tags")]
    public virtual bool? Tags { get; set; }

    [CliOption("--jobs", Format = OptionFormat.EqualsSeparated)]
    public string? Jobs { get; set; }

    [CliFlag("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [CliOption("--upload-pack", Format = OptionFormat.EqualsSeparated)]
    public string? UploadPack { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliOption("--server-option", Format = OptionFormat.EqualsSeparated)]
    public string? ServerOption { get; set; }

    [CliFlag("--show-forced-updates")]
    public virtual bool? ShowForcedUpdates { get; set; }

    [CliFlag("--no-show-forced-updates")]
    public virtual bool? NoShowForcedUpdates { get; set; }

    [CliFlag("--ipv4")]
    public virtual bool? Ipv4 { get; set; }

    [CliFlag("--ipv6")]
    public virtual bool? Ipv6 { get; set; }
}