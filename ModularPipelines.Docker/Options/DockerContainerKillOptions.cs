using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container kill")]
public record DockerContainerKillOptions : DockerOptions
{
    [CommandLongSwitch("signal")]
    public string? Signal { get; set; }

}