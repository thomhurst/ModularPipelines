using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx debug-shell")]
public record DockerBuildxDebugShellOptions : DockerOptions
{
    [BooleanCommandSwitch("detach")]
    public bool? Detach { get; set; }

    [CommandLongSwitch("progress")]
    public string? Progress { get; set; }

    [CommandLongSwitch("root")]
    public string? Root { get; set; }

    [CommandLongSwitch("server-config")]
    public string? ServerConfig { get; set; }

    [CommandLongSwitch("builder")]
    public string? Builder { get; set; }

}