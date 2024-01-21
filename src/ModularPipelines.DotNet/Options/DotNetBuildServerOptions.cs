using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("build-server", "shutdown")]
[ExcludeFromCodeCoverage]
public record DotNetBuildServerOptions : DotNetOptions
{
    [BooleanCommandSwitch("--msbuild")]
    public bool? Msbuild { get; set; }

    [BooleanCommandSwitch("--razor")]
    public bool? Razor { get; set; }

    [BooleanCommandSwitch("--vbcscompiler")]
    public bool? Vbcscompiler { get; set; }
}
