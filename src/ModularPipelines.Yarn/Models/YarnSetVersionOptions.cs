using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("set", "version")]
public record YarnSetVersionOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Version
) : YarnOptions
{
    [BooleanCommandSwitch("--yarn-path")]
    public virtual bool? YarnPath { get; set; }

    [BooleanCommandSwitch("--only-if-needed")]
    public virtual bool? OnlyIfNeeded { get; set; }
}