using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container rename")]
public record DockerContainerRenameOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container, [property: PositionalArgument(Position = Position.AfterArguments)] string Newname) : DockerOptions
{
}