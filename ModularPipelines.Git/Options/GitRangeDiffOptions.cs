using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("range-diff")]
public record GitRangeDiffOptions : GitOptions
{
    [BooleanCommandSwitch("no-dual-color")]
    public bool? NoDualColor { get; set; }

    [CommandLongSwitch("creation-factor")]
    public string? CreationFactor { get; set; }

    [BooleanCommandSwitch("left-only")]
    public bool? LeftOnly { get; set; }

    [BooleanCommandSwitch("right-only")]
    public bool? RightOnly { get; set; }

    [CommandLongSwitch("no-notes")]
    public string? NoNotes { get; set; }

    [CommandLongSwitch("notes")]
    public string? Notes { get; set; }

}