using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux;

[ExcludeFromCodeCoverage]
[CliTool("dpkg")]
public record DpkgInstallOptions : CommandLineToolOptions
{
    public DpkgInstallOptions(string path)
    {
        Path = path;
    }

    [CliOption("-i")]
    public virtual string Path { get; init; }
}