using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container rm")]
public record DockerContainerRmOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("link")]
    public string? Link { get; set; }

    [CommandLongSwitch("volumes")]
    public string? Volumes { get; set; }

}