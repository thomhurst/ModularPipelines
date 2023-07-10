using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx imagetools create")]
public record DockerBuildxImagetoolsCreateOptions : DockerOptions
{
    [CommandLongSwitch("progress")]
    public string? Progress { get; set; }

}