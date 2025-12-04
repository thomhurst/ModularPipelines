using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("rebase")]
[ExcludeFromCodeCoverage]
public record GitRebaseOptions : GitOptions
{
    [CliFlag("--continue")]
    public virtual bool? Continue { get; set; }

    [CliFlag("--skip")]
    public virtual bool? Skip { get; set; }

    [CliFlag("--abort")]
    public virtual bool? Abort { get; set; }

    [CliFlag("--quit")]
    public virtual bool? Quit { get; set; }

    [CliFlag("--edit-todo")]
    public virtual bool? EditTodo { get; set; }

    [CliFlag("--show-current-patch")]
    public virtual bool? ShowCurrentPatch { get; set; }

    [CliOption("--onto", Format = OptionFormat.EqualsSeparated)]
    public string? Onto { get; set; }

    [CliFlag("--keep-base")]
    public virtual bool? KeepBase { get; set; }

    [CliFlag("--apply")]
    public virtual bool? Apply { get; set; }

    [CliFlag("--empty")]
    public virtual bool? Empty { get; set; }

    [CliFlag("--no-keep-empty")]
    public virtual bool? NoKeepEmpty { get; set; }

    [CliFlag("--keep-empty")]
    public virtual bool? KeepEmpty { get; set; }

    [CliFlag("--reapply-cherry-picks")]
    public virtual bool? ReapplyCherryPicks { get; set; }

    [CliFlag("--no-reapply-cherry-picks")]
    public virtual bool? NoReapplyCherryPicks { get; set; }

    [CliFlag("--allow-empty-message")]
    public virtual bool? AllowEmptyMessage { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliOption("--strategy", Format = OptionFormat.EqualsSeparated)]
    public string? Strategy { get; set; }

    [CliOption("--strategy-option", Format = OptionFormat.EqualsSeparated)]
    public string? StrategyOption { get; set; }

    [CliFlag("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [CliFlag("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--stat")]
    public virtual bool? Stat { get; set; }

    [CliFlag("--no-stat")]
    public virtual bool? NoStat { get; set; }

    [CliFlag("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliFlag("--no-ff")]
    public virtual bool? NoFf { get; set; }

    [CliFlag("--force-rebase")]
    public virtual bool? ForceRebase { get; set; }

    [CliFlag("--fork-point")]
    public virtual bool? ForkPoint { get; set; }

    [CliFlag("--no-fork-point")]
    public virtual bool? NoForkPoint { get; set; }

    [CliFlag("--ignore-whitespace")]
    public virtual bool? IgnoreWhitespace { get; set; }

    [CliOption("--whitespace", Format = OptionFormat.EqualsSeparated)]
    public string? Whitespace { get; set; }

    [CliFlag("--committer-date-is-author-date")]
    public virtual bool? CommitterDateIsAuthorDate { get; set; }

    [CliFlag("--ignore-date")]
    public virtual bool? IgnoreDate { get; set; }

    [CliFlag("--reset-author-date")]
    public virtual bool? ResetAuthorDate { get; set; }

    [CliFlag("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--rebase-merges")]
    public virtual bool? RebaseMerges { get; set; }

    [CliFlag("--no-rebase-merges")]
    public virtual bool? NoRebaseMerges { get; set; }

    [CliOption("--exec", Format = OptionFormat.EqualsSeparated)]
    public string? Exec { get; set; }

    [CliFlag("--root")]
    public virtual bool? Root { get; set; }

    [CliFlag("--autosquash")]
    public virtual bool? Autosquash { get; set; }

    [CliFlag("--no-autosquash")]
    public virtual bool? NoAutosquash { get; set; }

    [CliFlag("--autostash")]
    public virtual bool? Autostash { get; set; }

    [CliFlag("--no-autostash")]
    public virtual bool? NoAutostash { get; set; }

    [CliFlag("--reschedule-failed-exec")]
    public virtual bool? RescheduleFailedExec { get; set; }

    [CliFlag("--no-reschedule-failed-exec")]
    public virtual bool? NoRescheduleFailedExec { get; set; }

    [CliFlag("--update-refs")]
    public virtual bool? UpdateRefs { get; set; }

    [CliFlag("--no-update-refs")]
    public virtual bool? NoUpdateRefs { get; set; }
}