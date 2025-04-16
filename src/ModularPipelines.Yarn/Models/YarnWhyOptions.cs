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
    public virtual bool? Recursive { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--peers")]
    public virtual bool? Peers { get; set; }
}