using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

public record AptGetPackageOptions : AptGetOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "package";
}