using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx version")]
public record DockerBuildxVersionOptions : DockerOptions
{
}