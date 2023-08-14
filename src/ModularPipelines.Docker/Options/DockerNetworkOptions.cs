using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network")]
public record DockerNetworkOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}