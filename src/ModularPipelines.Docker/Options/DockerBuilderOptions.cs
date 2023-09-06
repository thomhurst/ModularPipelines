using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("builder")]
[ExcludeFromCodeCoverage]
public record DockerBuilderOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}
