using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm join-token")]
public record DockerSwarmJoinTokenOptions : DockerOptions
{
    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("rotate")]
    public string? Rotate { get; set; }

}