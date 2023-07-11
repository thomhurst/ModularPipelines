using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("rebase")]
public record GitRebaseOptions : GitOptions
{
    [BooleanCommandSwitch("--continue")]
    public bool? Continue { get; set; }

    [BooleanCommandSwitch("--skip")]
    public bool? Skip { get; set; }

    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }

    [BooleanCommandSwitch("--quit")]
    public bool? Quit { get; set; }

    [BooleanCommandSwitch("--edit-todo")]
    public bool? EditTodo { get; set; }

    [BooleanCommandSwitch("--show-current-patch")]
    public bool? ShowCurrentPatch { get; set; }

    [CommandEqualsSeparatorSwitch("--onto")]
    public string? Onto { get; set; }

    [BooleanCommandSwitch("--keep-base")]
    public bool? KeepBase { get; set; }

    [BooleanCommandSwitch("--apply")]
    public bool? Apply { get; set; }

    [BooleanCommandSwitch("--empty")]
    public bool? Empty { get; set; }

    [BooleanCommandSwitch("--no-keep-empty")]
    public bool? NoKeepEmpty { get; set; }

    [BooleanCommandSwitch("--keep-empty")]
    public bool? KeepEmpty { get; set; }

    [BooleanCommandSwitch("--reapply-cherry-picks")]
    public bool? ReapplyCherryPicks { get; set; }

    [BooleanCommandSwitch("--no-reapply-cherry-picks")]
    public bool? NoReapplyCherryPicks { get; set; }

    [BooleanCommandSwitch("--allow-empty-message")]
    public bool? AllowEmptyMessage { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public bool? NoRerereAutoupdate { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--stat")]
    public bool? Stat { get; set; }

    [BooleanCommandSwitch("--no-stat")]
    public bool? NoStat { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [BooleanCommandSwitch("--no-ff")]
    public bool? NoFf { get; set; }

    [BooleanCommandSwitch("--force-rebase")]
    public bool? ForceRebase { get; set; }

    [BooleanCommandSwitch("--fork-point")]
    public bool? ForkPoint { get; set; }

    [BooleanCommandSwitch("--no-fork-point")]
    public bool? NoForkPoint { get; set; }

    [BooleanCommandSwitch("--ignore-whitespace")]
    public bool? IgnoreWhitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--whitespace")]
    public string? Whitespace { get; set; }

    [BooleanCommandSwitch("--committer-date-is-author-date")]
    public bool? CommitterDateIsAuthorDate { get; set; }

    [BooleanCommandSwitch("--ignore-date")]
    public bool? IgnoreDate { get; set; }

    [BooleanCommandSwitch("--reset-author-date")]
    public bool? ResetAuthorDate { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--rebase-merges")]
    public bool? RebaseMerges { get; set; }

    [BooleanCommandSwitch("--no-rebase-merges")]
    public bool? NoRebaseMerges { get; set; }

    [CommandEqualsSeparatorSwitch("--exec")]
    public string? Exec { get; set; }

    [BooleanCommandSwitch("--root")]
    public bool? Root { get; set; }

    [BooleanCommandSwitch("--autosquash")]
    public bool? Autosquash { get; set; }

    [BooleanCommandSwitch("--no-autosquash")]
    public bool? NoAutosquash { get; set; }

    [BooleanCommandSwitch("--autostash")]
    public bool? Autostash { get; set; }

    [BooleanCommandSwitch("--no-autostash")]
    public bool? NoAutostash { get; set; }

    [BooleanCommandSwitch("--reschedule-failed-exec")]
    public bool? RescheduleFailedExec { get; set; }

    [BooleanCommandSwitch("--no-reschedule-failed-exec")]
    public bool? NoRescheduleFailedExec { get; set; }

    [BooleanCommandSwitch("--update-refs")]
    public bool? UpdateRefs { get; set; }

    [BooleanCommandSwitch("--no-update-refs")]
    public bool? NoUpdateRefs { get; set; }
}