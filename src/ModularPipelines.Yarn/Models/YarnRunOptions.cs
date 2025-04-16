using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run")]
public record YarnRunOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string ScriptName
) : YarnOptions
{
    [BooleanCommandSwitch("--inspect")]
    public virtual bool? Inspect { get; set; }

    [BooleanCommandSwitch("--inspect-brk")]
    public virtual bool? InspectBrk { get; set; }

    [BooleanCommandSwitch("--top-level")]
    public virtual bool? TopLevel { get; set; }

    [BooleanCommandSwitch("--binaries-only")]
    public virtual bool? BinariesOnly { get; set; }

    [CommandSwitch("--require")]
    public virtual string? Require { get; set; }
}