using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("set", "resolution")]
public record YarnSetResolutionOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Descriptor,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Resolution
) : YarnOptions
{
    [BooleanCommandSwitch("--save")]
    public bool? Save { get; set; }
}