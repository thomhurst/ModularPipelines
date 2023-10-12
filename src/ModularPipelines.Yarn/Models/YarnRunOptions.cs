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
    public bool? Inspect { get; set; }

    [BooleanCommandSwitch("--inspect-brk")]
    public bool? InspectBrk { get; set; }

    [BooleanCommandSwitch("--top-level")]
    public bool? TopLevel { get; set; }

    [BooleanCommandSwitch("--binaries-only")]
    public bool? BinariesOnly { get; set; }

    [CommandSwitch("--require")]
    public string? Require { get; set; }
}