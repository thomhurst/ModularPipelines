using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("range-diff")]
[ExcludeFromCodeCoverage]
public record GitRangeDiffOptions : GitOptions
{
    [BooleanCommandSwitch("--no-dual-color")]
    public virtual bool? NoDualColor { get; set; }

    [CommandEqualsSeparatorSwitch("--creation-factor")]
    public string? CreationFactor { get; set; }

    [BooleanCommandSwitch("--left-only")]
    public virtual bool? LeftOnly { get; set; }

    [BooleanCommandSwitch("--right-only")]
    public virtual bool? RightOnly { get; set; }

    [CommandEqualsSeparatorSwitch("--no-notes")]
    public string? NoNotes { get; set; }

    [CommandEqualsSeparatorSwitch("--notes")]
    public string? Notes { get; set; }
}