using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("notes")]
[ExcludeFromCodeCoverage]
public record GitNotesOptions : GitOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--message", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Message { get; set; }

    [CliOption("--file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? File { get; set; }

    [CliOption("--reuse-message", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ReuseMessage { get; set; }

    [CliOption("--reedit-message", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ReeditMessage { get; set; }

    [CliFlag("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }

    [CliOption("--ref", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Ref { get; set; }

    [CliFlag("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliOption("--strategy", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Strategy { get; set; }

    [CliFlag("--commit")]
    public virtual bool? Commit { get; set; }

    [CliFlag("--abort")]
    public virtual bool? Abort { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }
}