using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx")]
public record DockerBuildxOptions : DockerOptions
{
}