using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetCheckOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "check";
}
