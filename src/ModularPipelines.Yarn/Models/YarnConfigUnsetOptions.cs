using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "unset")]
public record YarnConfigUnsetOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Name
) : YarnOptions
{
    [BooleanCommandSwitch("--home")]
    public bool? Home { get; set; }
}