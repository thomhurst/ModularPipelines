using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("why")]
public record YarnWhyOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Package
) : YarnOptions
{
    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--peers")]
    public bool? Peers { get; set; }
}