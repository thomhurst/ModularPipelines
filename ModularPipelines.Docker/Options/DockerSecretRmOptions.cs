using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret rm")]
public record DockerSecretRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Secret) : DockerOptions
{
}