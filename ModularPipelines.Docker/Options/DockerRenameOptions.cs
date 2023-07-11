using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("rename")]
public record DockerRenameOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container, [property: PositionalArgument(Position = Position.AfterArguments)] string Newname) : DockerOptions
{
}