using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("merge")]
[ExcludeFromCodeCoverage]
public record GitMergeOptions : GitOptions
{
    [CliFlag("--commit")]
    public virtual bool? Commit { get; set; }

    [CliFlag("--no-commit")]
    public virtual bool? NoCommit { get; set; }

    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliFlag("--no-edit")]
    public virtual bool? NoEdit { get; set; }

    [CliOption("--cleanup", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Cleanup { get; set; }

    [CliFlag("--ff")]
    public virtual bool? Ff { get; set; }

    [CliFlag("--no-ff")]
    public virtual bool? NoFf { get; set; }

    [CliFlag("--ff-only")]
    public virtual bool? FfOnly { get; set; }

    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public virtual string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [CliOption("--log", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Log { get; set; }

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
    public virtual string? Strategy { get; set; }

    [CliOption("--strategy-option", Format = OptionFormat.EqualsSeparated)]
    public virtual string? StrategyOption { get; set; }

    [CliFlag("--verify-signatures")]
    public virtual bool? VerifySignatures { get; set; }

    [CliFlag("--no-verify-signatures")]
    public virtual bool? NoVerifySignatures { get; set; }

    [CliFlag("--summary")]
    public virtual bool? Summary { get; set; }

    [CliFlag("--no-summary")]
    public virtual bool? NoSummary { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CliFlag("--autostash")]
    public virtual bool? Autostash { get; set; }

    [CliFlag("--no-autostash")]
    public virtual bool? NoAutostash { get; set; }

    [CliFlag("--allow-unrelated-histories")]
    public virtual bool? AllowUnrelatedHistories { get; set; }

    [CliOption("--into-name", Format = OptionFormat.EqualsSeparated)]
    public virtual string? IntoName { get; set; }

    [CliOption("--file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? File { get; set; }

    [CliFlag("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [CliFlag("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [CliFlag("--overwrite-ignore")]
    public virtual bool? OverwriteIgnore { get; set; }

    [CliFlag("--no-overwrite-ignore")]
    public virtual bool? NoOverwriteIgnore { get; set; }

    [CliFlag("--abort")]
    public virtual bool? Abort { get; set; }

    [CliFlag("--quit")]
    public virtual bool? Quit { get; set; }

    [CliFlag("--continue")]
    public virtual bool? Continue { get; set; }
}