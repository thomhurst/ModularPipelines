using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("kill")]
public record DockerKillOptions : DockerOptions
{
}