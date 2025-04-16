using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("build-server", "shutdown")]
[ExcludeFromCodeCoverage]
public record DotNetBuildServerOptions : DotNetOptions
{
    [BooleanCommandSwitch("--msbuild")]
    public virtual bool? Msbuild { get; set; }

    [BooleanCommandSwitch("--razor")]
    public virtual bool? Razor { get; set; }

    [BooleanCommandSwitch("--vbcscompiler")]
    public virtual bool? Vbcscompiler { get; set; }
}
