using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("am")]
public record GitAmOptions : GitOptions
{
    [BooleanCommandSwitch("--signoff")]
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("--keep")]
    public bool? Keep { get; set; }

    [BooleanCommandSwitch("--keep-non-patch")]
    public bool? KeepNonPatch { get; set; }

    [BooleanCommandSwitch("--no-keep-cr")]
    public bool? NoKeepCr { get; set; }

    [BooleanCommandSwitch("--keep-cr")]
    public bool? KeepCr { get; set; }

    [BooleanCommandSwitch("--c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("--scissors")]
    public bool? Scissors { get; set; }

    [BooleanCommandSwitch("--no-scissors")]
    public bool? NoScissors { get; set; }

    [CommandEqualsSeparatorSwitch("--quoted-cr")]
    public string? QuotedCr { get; set; }

    [BooleanCommandSwitch("--empty")]
    public bool? Empty { get; set; }

    [BooleanCommandSwitch("--message-id")]
    public bool? MessageId { get; set; }

    [BooleanCommandSwitch("--no-message-id")]
    public bool? NoMessageId { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--utf8")]
    public bool? Utf8 { get; set; }

    [BooleanCommandSwitch("--no-utf8")]
    public bool? NoUtf8 { get; set; }

    [BooleanCommandSwitch("--3way")]
    public bool? ThreeWay { get; set; }

    [BooleanCommandSwitch("--no-3way")]
    public bool? NoThreeway { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public bool? NoRerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--ignore-space-change")]
    public bool? IgnoreSpaceChange { get; set; }

    [BooleanCommandSwitch("--ignore-whitespace")]
    public bool? IgnoreWhitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--whitespace")]
    public string? Whitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--directory")]
    public string? Directory { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandEqualsSeparatorSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--reject")]
    public bool? Reject { get; set; }

    [BooleanCommandSwitch("--patch-format")]
    public bool? PatchFormat { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--committer-date-is-author-date")]
    public bool? CommitterDateIsAuthorDate { get; set; }

    [BooleanCommandSwitch("--ignore-date")]
    public bool? IgnoreDate { get; set; }

    [BooleanCommandSwitch("--skip")]
    public bool? Skip { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

    [BooleanCommandSwitch("--continue")]
    public bool? Continue { get; set; }

    [BooleanCommandSwitch("--resolved")]
    public bool? Resolved { get; set; }

    [CommandEqualsSeparatorSwitch("--resolvemsg")]
    public string? Resolvemsg { get; set; }

    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }

    [BooleanCommandSwitch("--quit")]
    public bool? Quit { get; set; }

    [BooleanCommandSwitch("--show-current-patch")]
    public bool? ShowCurrentPatch { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public bool? AllowEmpty { get; set; }
}