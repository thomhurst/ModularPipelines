using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume prune")]
public record DockerVolumePruneOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}