using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container")]
[ExcludeFromCodeCoverage]
public record DockerContainerOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}
