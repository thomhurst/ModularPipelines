using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("am")]
[ExcludeFromCodeCoverage]
public record GitAmOptions : GitOptions
{
    [BooleanCommandSwitch("--signoff")]
    public virtual bool? Signoff { get; set; }

    [BooleanCommandSwitch("--keep")]
    public virtual bool? Keep { get; set; }

    [BooleanCommandSwitch("--keep-non-patch")]
    public virtual bool? KeepNonPatch { get; set; }

    [BooleanCommandSwitch("--no-keep-cr")]
    public virtual bool? NoKeepCr { get; set; }

    [BooleanCommandSwitch("--keep-cr")]
    public virtual bool? KeepCr { get; set; }

    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--scissors")]
    public virtual bool? Scissors { get; set; }

    [BooleanCommandSwitch("--no-scissors")]
    public virtual bool? NoScissors { get; set; }

    [CommandEqualsSeparatorSwitch("--quoted-cr")]
    public string? QuotedCr { get; set; }

    [BooleanCommandSwitch("--empty")]
    public virtual bool? Empty { get; set; }

    [BooleanCommandSwitch("--message-id")]
    public virtual bool? MessageId { get; set; }

    [BooleanCommandSwitch("--no-message-id")]
    public virtual bool? NoMessageId { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--utf8")]
    public virtual bool? Utf8 { get; set; }

    [BooleanCommandSwitch("--no-utf8")]
    public virtual bool? NoUtf8 { get; set; }

    [BooleanCommandSwitch("--3way")]
    public virtual bool? ThreeWay { get; set; }

    [BooleanCommandSwitch("--no-3way")]
    public virtual bool? NoThreeway { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--ignore-space-change")]
    public virtual bool? IgnoreSpaceChange { get; set; }

    [BooleanCommandSwitch("--ignore-whitespace")]
    public virtual bool? IgnoreWhitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--whitespace")]
    public string? Whitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--directory")]
    public string? Directory { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandEqualsSeparatorSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--reject")]
    public virtual bool? Reject { get; set; }

    [BooleanCommandSwitch("--patch-format")]
    public virtual bool? PatchFormat { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--committer-date-is-author-date")]
    public virtual bool? CommitterDateIsAuthorDate { get; set; }

    [BooleanCommandSwitch("--ignore-date")]
    public virtual bool? IgnoreDate { get; set; }

    [BooleanCommandSwitch("--skip")]
    public virtual bool? Skip { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [BooleanCommandSwitch("--continue")]
    public virtual bool? Continue { get; set; }

    [BooleanCommandSwitch("--resolved")]
    public virtual bool? Resolved { get; set; }

    [CommandEqualsSeparatorSwitch("--resolvemsg")]
    public string? Resolvemsg { get; set; }

    [BooleanCommandSwitch("--abort")]
    public virtual bool? Abort { get; set; }

    [BooleanCommandSwitch("--quit")]
    public virtual bool? Quit { get; set; }

    [BooleanCommandSwitch("--show-current-patch")]
    public virtual bool? ShowCurrentPatch { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }
}