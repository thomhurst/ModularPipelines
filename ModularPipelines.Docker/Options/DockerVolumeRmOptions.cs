using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume rm")]
public record DockerVolumeRmOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}