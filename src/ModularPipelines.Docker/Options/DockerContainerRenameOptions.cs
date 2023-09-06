using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container rename")]
[ExcludeFromCodeCoverage]
public record DockerContainerRenameOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container, [property: PositionalArgument(Position = Position.AfterArguments)] string Newname) : DockerOptions
{
}
