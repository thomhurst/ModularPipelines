using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network prune")]
public record DockerNetworkPruneOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}