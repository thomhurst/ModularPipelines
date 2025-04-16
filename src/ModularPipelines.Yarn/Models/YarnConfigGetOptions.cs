using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "get")]
public record YarnConfigGetOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Name
) : YarnOptions
{
    [BooleanCommandSwitch("--why")]
    public virtual bool? Why { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--no-redacted")]
    public virtual bool? NoRedacted { get; set; }
}