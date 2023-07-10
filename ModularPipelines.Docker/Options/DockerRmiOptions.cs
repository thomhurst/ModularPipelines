using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("rmi")]
public record DockerRmiOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("no-prune")]
    public string? NoPrune { get; set; }

}