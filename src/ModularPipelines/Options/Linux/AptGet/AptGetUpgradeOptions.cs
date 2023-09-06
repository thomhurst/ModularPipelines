using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetUpgradeOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "upgrade";
}
