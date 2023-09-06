using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("wait")]
[ExcludeFromCodeCoverage]
public record DockerWaitOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Containers) : DockerOptions
{
}
