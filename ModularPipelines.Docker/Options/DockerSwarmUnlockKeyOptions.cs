using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm unlock-key")]
public record DockerSwarmUnlockKeyOptions : DockerOptions
{
    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("rotate")]
    public string? Rotate { get; set; }

}