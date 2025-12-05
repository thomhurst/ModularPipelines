using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliSubCommand("build-server", "shutdown")]
[ExcludeFromCodeCoverage]
public record DotNetBuildServerOptions : DotNetOptions
{
    [CliFlag("--msbuild")]
    public virtual bool? Msbuild { get; set; }

    [CliFlag("--razor")]
    public virtual bool? Razor { get; set; }

    [CliFlag("--vbcscompiler")]
    public virtual bool? Vbcscompiler { get; set; }
}
