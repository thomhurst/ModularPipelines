using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("rebase")]
[ExcludeFromCodeCoverage]
public record GitRebaseOptions : GitOptions
{
    [BooleanCommandSwitch("--continue")]
    public virtual bool? Continue { get; set; }

    [BooleanCommandSwitch("--skip")]
    public virtual bool? Skip { get; set; }

    [BooleanCommandSwitch("--abort")]
    public virtual bool? Abort { get; set; }

    [BooleanCommandSwitch("--quit")]
    public virtual bool? Quit { get; set; }

    [BooleanCommandSwitch("--edit-todo")]
    public virtual bool? EditTodo { get; set; }

    [BooleanCommandSwitch("--show-current-patch")]
    public virtual bool? ShowCurrentPatch { get; set; }

    [CommandEqualsSeparatorSwitch("--onto")]
    public string? Onto { get; set; }

    [BooleanCommandSwitch("--keep-base")]
    public virtual bool? KeepBase { get; set; }

    [BooleanCommandSwitch("--apply")]
    public virtual bool? Apply { get; set; }

    [BooleanCommandSwitch("--empty")]
    public virtual bool? Empty { get; set; }

    [BooleanCommandSwitch("--no-keep-empty")]
    public virtual bool? NoKeepEmpty { get; set; }

    [BooleanCommandSwitch("--keep-empty")]
    public virtual bool? KeepEmpty { get; set; }

    [BooleanCommandSwitch("--reapply-cherry-picks")]
    public virtual bool? ReapplyCherryPicks { get; set; }

    [BooleanCommandSwitch("--no-reapply-cherry-picks")]
    public virtual bool? NoReapplyCherryPicks { get; set; }

    [BooleanCommandSwitch("--allow-empty-message")]
    public virtual bool? AllowEmptyMessage { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--stat")]
    public virtual bool? Stat { get; set; }

    [BooleanCommandSwitch("--no-stat")]
    public virtual bool? NoStat { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }

    [BooleanCommandSwitch("--no-ff")]
    public virtual bool? NoFf { get; set; }

    [BooleanCommandSwitch("--force-rebase")]
    public virtual bool? ForceRebase { get; set; }

    [BooleanCommandSwitch("--fork-point")]
    public virtual bool? ForkPoint { get; set; }

    [BooleanCommandSwitch("--no-fork-point")]
    public virtual bool? NoForkPoint { get; set; }

    [BooleanCommandSwitch("--ignore-whitespace")]
    public virtual bool? IgnoreWhitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--whitespace")]
    public string? Whitespace { get; set; }

    [BooleanCommandSwitch("--committer-date-is-author-date")]
    public virtual bool? CommitterDateIsAuthorDate { get; set; }

    [BooleanCommandSwitch("--ignore-date")]
    public virtual bool? IgnoreDate { get; set; }

    [BooleanCommandSwitch("--reset-author-date")]
    public virtual bool? ResetAuthorDate { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public virtual bool? Signoff { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--rebase-merges")]
    public virtual bool? RebaseMerges { get; set; }

    [BooleanCommandSwitch("--no-rebase-merges")]
    public virtual bool? NoRebaseMerges { get; set; }

    [CommandEqualsSeparatorSwitch("--exec")]
    public string? Exec { get; set; }

    [BooleanCommandSwitch("--root")]
    public virtual bool? Root { get; set; }

    [BooleanCommandSwitch("--autosquash")]
    public virtual bool? Autosquash { get; set; }

    [BooleanCommandSwitch("--no-autosquash")]
    public virtual bool? NoAutosquash { get; set; }

    [BooleanCommandSwitch("--autostash")]
    public virtual bool? Autostash { get; set; }

    [BooleanCommandSwitch("--no-autostash")]
    public virtual bool? NoAutostash { get; set; }

    [BooleanCommandSwitch("--reschedule-failed-exec")]
    public virtual bool? RescheduleFailedExec { get; set; }

    [BooleanCommandSwitch("--no-reschedule-failed-exec")]
    public virtual bool? NoRescheduleFailedExec { get; set; }

    [BooleanCommandSwitch("--update-refs")]
    public virtual bool? UpdateRefs { get; set; }

    [BooleanCommandSwitch("--no-update-refs")]
    public virtual bool? NoUpdateRefs { get; set; }
}