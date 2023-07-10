using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("rm")]
public record DockerRmOptions : DockerOptions
{
}