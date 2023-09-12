using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux;

[ExcludeFromCodeCoverage]
public record DpkgInstallOptions : CommandLineToolOptions
{
    public DpkgInstallOptions(string path) : base("dpkg")
    {
        Path = path;
        Sudo = true;
    }

    [CommandSwitch("-i")] 
    public string Path { get; init; }
}
