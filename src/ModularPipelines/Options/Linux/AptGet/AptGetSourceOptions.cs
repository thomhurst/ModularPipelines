using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetSourceOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string CommandName { get; } = "source";
}