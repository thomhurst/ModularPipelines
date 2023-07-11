using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("info")]
public record DockerInfoOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

}