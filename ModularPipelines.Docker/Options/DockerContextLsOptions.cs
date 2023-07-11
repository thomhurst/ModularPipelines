using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context ls")]
public record DockerContextLsOptions : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

}