using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("search")]
public record DockerSearchOptions : DockerOptions
{
}