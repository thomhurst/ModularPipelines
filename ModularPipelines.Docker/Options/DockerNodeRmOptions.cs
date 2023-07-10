using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node rm")]
public record DockerNodeRmOptions : DockerOptions
{
}