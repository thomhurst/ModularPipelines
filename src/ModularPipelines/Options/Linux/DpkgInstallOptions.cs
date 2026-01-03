using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux;

[ExcludeFromCodeCoverage]
public record DpkgInstallOptions : CommandLineToolOptions
{
    public DpkgInstallOptions(string path) : base("dpkg")
    {
        Path = path;
    }

    [CliOption("-i")]
    public virtual string Path { get; init; }
}