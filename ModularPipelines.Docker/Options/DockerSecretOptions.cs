using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret")]
public record DockerSecretOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}