using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx create")]
public record DockerBuildxCreateOptions : DockerOptions
{
    [CommandLongSwitch("bootstrap")]
    public string? Bootstrap { get; set; }

}