using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin rm")]
public record DockerPluginRmOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}