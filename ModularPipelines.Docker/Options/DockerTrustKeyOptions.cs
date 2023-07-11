using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust key")]
public record DockerTrustKeyOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}