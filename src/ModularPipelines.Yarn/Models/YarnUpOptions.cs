using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("up")]
public record YarnUpOptions : YarnOptions
{
    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--fixed")]
    public virtual bool? Fixed { get; set; }

    [BooleanCommandSwitch("--exact")]
    public virtual bool? Exact { get; set; }

    [BooleanCommandSwitch("--tilde")]
    public virtual bool? Tilde { get; set; }

    [BooleanCommandSwitch("--caret")]
    public virtual bool? Caret { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CommandSwitch("--mode")]
    public virtual string? Mode { get; set; }
}