using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx inspect")]
public record DockerBuildxInspectOptions : DockerOptions
{
}