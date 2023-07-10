using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin enable")]
public record DockerPluginEnableOptions : DockerOptions
{
    [CommandLongSwitch("timeout")]
    public string? Timeout { get; set; }

}