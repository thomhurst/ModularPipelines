using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume update")]
public record DockerVolumeUpdateOptions : DockerOptions
{
    [CommandLongSwitch("availability")]
    public string? Availability { get; set; }

}