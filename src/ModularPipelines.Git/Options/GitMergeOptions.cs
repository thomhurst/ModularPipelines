using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("merge")]
[ExcludeFromCodeCoverage]
public record GitMergeOptions : GitOptions
{
    [BooleanCommandSwitch("--commit")]
    public bool? Commit { get; set; }

    [BooleanCommandSwitch("--no-commit")]
    public bool? NoCommit { get; set; }

    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("--no-edit")]
    public bool? NoEdit { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("--ff")]
    public bool? Ff { get; set; }

    [BooleanCommandSwitch("--no-ff")]
    public bool? NoFf { get; set; }

    [BooleanCommandSwitch("--ff-only")]
    public bool? FfOnly { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

    [CommandEqualsSeparatorSwitch("--log")]
    public string? Log { get; set; }

    [BooleanCommandSwitch("--no-log")]
    public bool? NoLog { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("--no-signoff")]
    public bool? NoSignoff { get; set; }

    [BooleanCommandSwitch("--stat")]
    public bool? Stat { get; set; }

    [BooleanCommandSwitch("--no-stat")]
    public bool? NoStat { get; set; }

    [BooleanCommandSwitch("--squash")]
    public bool? Squash { get; set; }

    [BooleanCommandSwitch("--no-squash")]
    public bool? NoSquash { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("--verify-signatures")]
    public bool? VerifySignatures { get; set; }

    [BooleanCommandSwitch("--no-verify-signatures")]
    public bool? NoVerifySignatures { get; set; }

    [BooleanCommandSwitch("--summary")]
    public bool? Summary { get; set; }

    [BooleanCommandSwitch("--no-summary")]
    public bool? NoSummary { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--autostash")]
    public bool? Autostash { get; set; }

    [BooleanCommandSwitch("--no-autostash")]
    public bool? NoAutostash { get; set; }

    [BooleanCommandSwitch("--allow-unrelated-histories")]
    public bool? AllowUnrelatedHistories { get; set; }

    [CommandEqualsSeparatorSwitch("--into-name")]
    public string? IntoName { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public bool? NoRerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--overwrite-ignore")]
    public bool? OverwriteIgnore { get; set; }

    [BooleanCommandSwitch("--no-overwrite-ignore")]
    public bool? NoOverwriteIgnore { get; set; }

    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }

    [BooleanCommandSwitch("--quit")]
    public bool? Quit { get; set; }

    [BooleanCommandSwitch("--continue")]
    public bool? Continue { get; set; }
}
