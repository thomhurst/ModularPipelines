using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetAutocleanOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "autoclean";
}
