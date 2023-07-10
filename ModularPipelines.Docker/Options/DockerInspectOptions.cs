using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("inspect")]
public record DockerInspectOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("size")]
    public string? Size { get; set; }

    [CommandLongSwitch("type")]
    public string? Type { get; set; }

}