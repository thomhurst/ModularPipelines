using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image load")]
public record DockerImageLoadOptions : DockerOptions
{
    [CommandLongSwitch("input")]
    public string? Input { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}