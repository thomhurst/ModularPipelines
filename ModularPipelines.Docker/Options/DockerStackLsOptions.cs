using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack ls")]
public record DockerStackLsOptions : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}