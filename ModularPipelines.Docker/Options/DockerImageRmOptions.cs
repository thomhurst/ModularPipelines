using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image rm")]
public record DockerImageRmOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

    [CommandLongSwitch("no-prune")]
    public string? NoPrune { get; set; }

}