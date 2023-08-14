using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

public record AptGetCheckOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "check";
}