using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack")]
public record DockerStackOptions : DockerOptions
{
}