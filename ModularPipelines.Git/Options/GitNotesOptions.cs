using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("notes")]
public record GitNotesOptions : GitOptions
{
    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [CommandLongSwitch("message")]
    public string? Message { get; set; }

    [CommandLongSwitch("file")]
    public string? File { get; set; }

    [CommandLongSwitch("reuse-message")]
    public string? ReuseMessage { get; set; }

    [CommandLongSwitch("reedit-message")]
    public string? ReeditMessage { get; set; }

    [BooleanCommandSwitch("allow-empty")]
    public bool? AllowEmpty { get; set; }

    [CommandLongSwitch("ref")]
    public string? Ref { get; set; }

    [BooleanCommandSwitch("ignore-missing")]
    public bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [CommandLongSwitch("strategy")]
    public string? Strategy { get; set; }

    [BooleanCommandSwitch("commit")]
    public bool? Commit { get; set; }

    [BooleanCommandSwitch("abort")]
    public bool? Abort { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

}