using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("version")]
public record YarnVersionOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Strategy
) : YarnOptions
{
    [BooleanCommandSwitch("--deferred")]
    public virtual bool? Deferred { get; set; }

    [BooleanCommandSwitch("--immediate")]
    public virtual bool? Immediate { get; set; }
}