using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("notes")]
public record GitNotesOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--message")]
    public string? Message { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [CommandEqualsSeparatorSwitch("--reuse-message")]
    public string? ReuseMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--reedit-message")]
    public string? ReeditMessage { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public bool? AllowEmpty { get; set; }

    [CommandEqualsSeparatorSwitch("--ref")]
    public string? Ref { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [BooleanCommandSwitch("--commit")]
    public bool? Commit { get; set; }

    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }
}