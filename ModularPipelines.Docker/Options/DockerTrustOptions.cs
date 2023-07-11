using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust")]
public record DockerTrustOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}