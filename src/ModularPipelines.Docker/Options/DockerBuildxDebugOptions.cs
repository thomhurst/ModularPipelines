using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "debug")]
[ExcludeFromCodeCoverage]
public record DockerBuildxDebugOptions : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandSwitch("--invoke")]
    public virtual string? Invoke { get; set; }

    [CommandSwitch("--on")]
    public virtual string? On { get; set; }

    [CommandSwitch("--progress")]
    public virtual string? Progress { get; set; }

    [CommandSwitch("--root")]
    public virtual string? Root { get; set; }

    [CommandSwitch("--server-config")]
    public virtual string? ServerConfig { get; set; }
}
