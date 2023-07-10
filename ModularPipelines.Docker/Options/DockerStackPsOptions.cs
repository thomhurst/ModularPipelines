using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack ps")]
public record DockerStackPsOptions : DockerOptions
{
}