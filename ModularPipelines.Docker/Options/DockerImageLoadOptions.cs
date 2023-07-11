using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image load")]
public record DockerImageLoadOptions : DockerOptions
{

    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}