using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("restart")]
public record DockerRestartOptions(string Container) : DockerOptions
{
    [CommandLongSwitch("signal")]
    public string? Signal { get; set; }

    [CommandLongSwitch("time")]
    public string? Time { get; set; }

}