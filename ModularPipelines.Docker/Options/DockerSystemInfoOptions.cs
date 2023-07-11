using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system info")]
public record DockerSystemInfoOptions : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }

}