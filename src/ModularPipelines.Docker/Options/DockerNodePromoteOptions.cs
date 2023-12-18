using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node promote")]
[ExcludeFromCodeCoverage]
public record DockerNodePromoteOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Node) : DockerOptions
{
}