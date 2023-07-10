using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm leave")]
public record DockerSwarmLeaveOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}