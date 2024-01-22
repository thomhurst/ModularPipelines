using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "debug")]
[ExcludeFromCodeCoverage]
public record DockerBuildxDebugOptions : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [CommandSwitch("--invoke")]
    public string? Invoke { get; set; }

    [CommandSwitch("--on")]
    public string? On { get; set; }

    [CommandSwitch("--progress")]
    public string? Progress { get; set; }

    [CommandSwitch("--root")]
    public string? Root { get; set; }

    [CommandSwitch("--server-config")]
    public string? ServerConfig { get; set; }
}
