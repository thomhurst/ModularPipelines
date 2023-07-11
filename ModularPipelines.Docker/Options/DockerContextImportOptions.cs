using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context import")]
public record DockerContextImportOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Context, [property: PositionalArgument(Position = Position.AfterArguments)] string File) : DockerOptions
{
}