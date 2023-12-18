using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetInstallOptions : AptGetOptions
{
    public AptGetInstallOptions(string package)
    {
        Package = package;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string CommandName { get; } = "install";

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string Package { get; }
}