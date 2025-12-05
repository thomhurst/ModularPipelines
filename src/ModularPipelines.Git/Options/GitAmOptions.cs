using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("am")]
[ExcludeFromCodeCoverage]
public record GitAmOptions : GitOptions
{
    [CliFlag("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CliFlag("--keep")]
    public virtual bool? Keep { get; set; }

    [CliFlag("--keep-non-patch")]
    public virtual bool? KeepNonPatch { get; set; }

    [CliFlag("--no-keep-cr")]
    public virtual bool? NoKeepCr { get; set; }

    [CliFlag("--keep-cr")]
    public virtual bool? KeepCr { get; set; }

    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--scissors")]
    public virtual bool? Scissors { get; set; }

    [CliFlag("--no-scissors")]
    public virtual bool? NoScissors { get; set; }

    [CliOption("--quoted-cr", Format = OptionFormat.EqualsSeparated)]
    public virtual string? QuotedCr { get; set; }

    [CliFlag("--empty")]
    public virtual bool? Empty { get; set; }

    [CliFlag("--message-id")]
    public virtual bool? MessageId { get; set; }

    [CliFlag("--no-message-id")]
    public virtual bool? NoMessageId { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--utf8")]
    public virtual bool? Utf8 { get; set; }

    [CliFlag("--no-utf8")]
    public virtual bool? NoUtf8 { get; set; }

    [CliFlag("--3way")]
    public virtual bool? ThreeWay { get; set; }

    [CliFlag("--no-3way")]
    public virtual bool? NoThreeway { get; set; }

    [CliFlag("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [CliFlag("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [CliFlag("--ignore-space-change")]
    public virtual bool? IgnoreSpaceChange { get; set; }

    [CliFlag("--ignore-whitespace")]
    public virtual bool? IgnoreWhitespace { get; set; }

    [CliOption("--whitespace", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Whitespace { get; set; }

    [CliOption("--directory", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Directory { get; set; }

    [CliOption("--exclude", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Exclude { get; set; }

    [CliOption("--include", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Include { get; set; }

    [CliFlag("--reject")]
    public virtual bool? Reject { get; set; }

    [CliFlag("--patch-format")]
    public virtual bool? PatchFormat { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [CliFlag("--committer-date-is-author-date")]
    public virtual bool? CommitterDateIsAuthorDate { get; set; }

    [CliFlag("--ignore-date")]
    public virtual bool? IgnoreDate { get; set; }

    [CliFlag("--skip")]
    public virtual bool? Skip { get; set; }

    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public virtual string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [CliFlag("--continue")]
    public virtual bool? Continue { get; set; }

    [CliFlag("--resolved")]
    public virtual bool? Resolved { get; set; }

    [CliOption("--resolvemsg", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Resolvemsg { get; set; }

    [CliFlag("--abort")]
    public virtual bool? Abort { get; set; }

    [CliFlag("--quit")]
    public virtual bool? Quit { get; set; }

    [CliFlag("--show-current-patch")]
    public virtual bool? ShowCurrentPatch { get; set; }

    [CliFlag("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }
}