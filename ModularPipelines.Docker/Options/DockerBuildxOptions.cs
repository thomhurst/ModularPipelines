using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx")]
public record DockerBuildxOptions : DockerOptions
{

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}