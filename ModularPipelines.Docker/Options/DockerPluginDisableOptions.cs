using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin disable")]
public record DockerPluginDisableOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}