using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("up")]
public record YarnUpOptions : YarnOptions
{
    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--fixed")]
    public bool? Fixed { get; set; }

    [BooleanCommandSwitch("--exact")]
    public bool? Exact { get; set; }

    [BooleanCommandSwitch("--tilde")]
    public bool? Tilde { get; set; }

    [BooleanCommandSwitch("--caret")]
    public bool? Caret { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }
}