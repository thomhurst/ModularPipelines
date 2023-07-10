using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx rm")]
public record DockerBuildxRmOptions : DockerOptions
{
}