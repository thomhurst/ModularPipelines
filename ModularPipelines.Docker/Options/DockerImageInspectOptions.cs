using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image inspect")]
public record DockerImageInspectOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

}