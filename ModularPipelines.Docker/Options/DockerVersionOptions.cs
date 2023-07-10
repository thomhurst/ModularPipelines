using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("version")]
public record DockerVersionOptions : DockerOptions
{
}