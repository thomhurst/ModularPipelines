using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack ls")]
public record DockerStackLsOptions : DockerOptions
{
}