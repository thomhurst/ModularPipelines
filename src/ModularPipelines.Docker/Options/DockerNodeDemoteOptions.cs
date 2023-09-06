using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node demote")]
[ExcludeFromCodeCoverage]
public record DockerNodeDemoteOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Node) : DockerOptions
{
}
