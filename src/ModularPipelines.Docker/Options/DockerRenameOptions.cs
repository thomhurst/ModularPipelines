using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("rename")]
[ExcludeFromCodeCoverage]
public record DockerRenameOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container, [property: PositionalArgument(Position = Position.AfterSwitches)] string Newname) : DockerOptions
{
}
