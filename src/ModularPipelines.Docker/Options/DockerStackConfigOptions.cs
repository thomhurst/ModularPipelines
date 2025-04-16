using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack", "config")]
[ExcludeFromCodeCoverage]
public record DockerStackConfigOptions : DockerOptions
{
    [CommandSwitch("--compose-file")]
    public virtual string? ComposeFile { get; set; }

    [CommandSwitch("--skip-interpolation")]
    public virtual string? SkipInterpolation { get; set; }
}
