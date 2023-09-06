using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("range-diff")]
[ExcludeFromCodeCoverage]
public record GitRangeDiffOptions : GitOptions
{
    [BooleanCommandSwitch("--no-dual-color")]
    public bool? NoDualColor { get; set; }

    [CommandEqualsSeparatorSwitch("--creation-factor")]
    public string? CreationFactor { get; set; }

    [BooleanCommandSwitch("--left-only")]
    public bool? LeftOnly { get; set; }

    [BooleanCommandSwitch("--right-only")]
    public bool? RightOnly { get; set; }

    [CommandEqualsSeparatorSwitch("--no-notes")]
    public string? NoNotes { get; set; }

    [CommandEqualsSeparatorSwitch("--notes")]
    public string? Notes { get; set; }
}
