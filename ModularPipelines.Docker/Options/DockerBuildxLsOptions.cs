using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx ls")]
public record DockerBuildxLsOptions : DockerOptions
{
}