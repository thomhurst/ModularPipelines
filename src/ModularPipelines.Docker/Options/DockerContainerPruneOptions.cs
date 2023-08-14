using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container prune")]
public record DockerContainerPruneOptions : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}