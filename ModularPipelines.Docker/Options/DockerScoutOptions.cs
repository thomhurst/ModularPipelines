using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout")]
public record DockerScoutOptions : DockerOptions
{
}