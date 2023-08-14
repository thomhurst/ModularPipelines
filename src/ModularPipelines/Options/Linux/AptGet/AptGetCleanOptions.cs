using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

public record AptGetCleanOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "clean";
}