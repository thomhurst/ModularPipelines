using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container prune")]
public record DockerContainerPruneOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}