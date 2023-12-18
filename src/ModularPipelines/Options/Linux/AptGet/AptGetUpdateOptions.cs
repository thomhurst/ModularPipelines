using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetUpdateOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string CommandName { get; } = "update";
}