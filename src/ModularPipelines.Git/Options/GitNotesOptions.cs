using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("notes")]
[ExcludeFromCodeCoverage]
public record GitNotesOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--message")]
    public string? Message { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [CommandEqualsSeparatorSwitch("--reuse-message")]
    public string? ReuseMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--reedit-message")]
    public string? ReeditMessage { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }

    [CommandEqualsSeparatorSwitch("--ref")]
    public string? Ref { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [BooleanCommandSwitch("--commit")]
    public virtual bool? Commit { get; set; }

    [BooleanCommandSwitch("--abort")]
    public virtual bool? Abort { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }
}