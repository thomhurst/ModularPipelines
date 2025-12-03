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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string CommandName { get; } = "install";

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string Package { get; }
}