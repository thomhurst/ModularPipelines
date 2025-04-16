using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("merge")]
[ExcludeFromCodeCoverage]
public record GitMergeOptions : GitOptions
{
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

    [BooleanCommandSwitch("--ff")]
    public virtual bool? Ff { get; set; }

    [BooleanCommandSwitch("--no-ff")]
    public virtual bool? NoFf { get; set; }

    [BooleanCommandSwitch("--ff-only")]
    public virtual bool? FfOnly { get; set; }

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

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--autostash")]
    public virtual bool? Autostash { get; set; }

    [BooleanCommandSwitch("--no-autostash")]
    public virtual bool? NoAutostash { get; set; }

    [BooleanCommandSwitch("--allow-unrelated-histories")]
    public virtual bool? AllowUnrelatedHistories { get; set; }

    [CommandEqualsSeparatorSwitch("--into-name")]
    public string? IntoName { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--overwrite-ignore")]
    public virtual bool? OverwriteIgnore { get; set; }

    [BooleanCommandSwitch("--no-overwrite-ignore")]
    public virtual bool? NoOverwriteIgnore { get; set; }

    [BooleanCommandSwitch("--abort")]
    public virtual bool? Abort { get; set; }

    [BooleanCommandSwitch("--quit")]
    public virtual bool? Quit { get; set; }

    [BooleanCommandSwitch("--continue")]
    public virtual bool? Continue { get; set; }
}