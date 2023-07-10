using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service inspect")]
public record DockerServiceInspectOptions : DockerOptions
{
}