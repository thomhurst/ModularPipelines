using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container restart")]
public record DockerContainerRestartOptions : DockerOptions
{
    [CommandLongSwitch("signal")]
    public string? Signal { get; set; }

    [CommandLongSwitch("time")]
    public string? Time { get; set; }

}