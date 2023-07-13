using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

public record AptGetInstallOptions : AptGetOptions
{
    public AptGetInstallOptions(string package)
    {
        Package = package;
    }
    [PositionalArgument(Position = Position.AfterArguments)]
    public string CommandName { get; } = "install";
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Package { get; }
}