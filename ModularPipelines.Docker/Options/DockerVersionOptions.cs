using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("version")]
public record DockerVersionOptions : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }

}