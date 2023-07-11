using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx debug-shell")]
public record DockerBuildxDebugShellOptions : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }


    [CommandSwitch("--progress")]
    public string? Progress { get; set; }


    [CommandSwitch("--root")]
    public string? Root { get; set; }


    [CommandSwitch("--server-config")]
    public string? ServerConfig { get; set; }


    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

}