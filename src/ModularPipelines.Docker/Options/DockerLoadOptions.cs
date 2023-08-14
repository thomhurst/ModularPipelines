using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("load")]
public record DockerLoadOptions : DockerOptions
{
    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--input")]
    public string? Input { get; set; }
}