using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config")]
[ExcludeFromCodeCoverage]
public record DockerConfigOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions
{
}
