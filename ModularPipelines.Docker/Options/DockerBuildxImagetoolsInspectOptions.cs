using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx imagetools inspect")]
public record DockerBuildxImagetoolsInspectOptions : DockerOptions
{
}