using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack")]
[ExcludeFromCodeCoverage]
public record DockerStackOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions
{
}