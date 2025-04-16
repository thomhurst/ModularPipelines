using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "set")]
public record YarnConfigSetOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Name,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
) : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--home")]
    public virtual bool? Home { get; set; }
}