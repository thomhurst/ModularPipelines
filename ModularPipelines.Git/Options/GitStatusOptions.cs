using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("status")]
public record GitStatusOptions : GitOptions
{
    [BooleanCommandSwitch("short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("branch")]
    public bool? Branch { get; set; }

    [BooleanCommandSwitch("show-stash")]
    public bool? ShowStash { get; set; }

    [CommandLongSwitch("porcelain")]
    public string? Porcelain { get; set; }

    [BooleanCommandSwitch("long")]
    public bool? Long { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [CommandLongSwitch("untracked-files")]
    public string? UntrackedFiles { get; set; }

    [CommandLongSwitch("ignore-submodules")]
    public string? IgnoreSubmodules { get; set; }

    [CommandLongSwitch("ignored")]
    public string? Ignored { get; set; }

    [CommandLongSwitch("column")]
    public string? Column { get; set; }

    [BooleanCommandSwitch("no-column")]
    public bool? NoColumn { get; set; }

    [BooleanCommandSwitch("ahead-behind")]
    public bool? AheadBehind { get; set; }

    [BooleanCommandSwitch("no-ahead-behind")]
    public bool? NoAheadBehind { get; set; }

    [BooleanCommandSwitch("renames")]
    public bool? Renames { get; set; }

    [BooleanCommandSwitch("no-renames")]
    public bool? NoRenames { get; set; }

    [CommandLongSwitch("find-renames")]
    public string? FindRenames { get; set; }

}