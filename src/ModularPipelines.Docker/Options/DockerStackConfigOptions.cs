using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack config")]
[ExcludeFromCodeCoverage]
public record DockerStackConfigOptions : DockerOptions
{
    [CommandSwitch("--compose-file")]
    public string? ComposeFile { get; set; }

    [CommandSwitch("--skip-interpolation")]
    public string? SkipInterpolation { get; set; }
}
