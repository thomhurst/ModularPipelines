using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context use")]
[ExcludeFromCodeCoverage]
public record DockerContextUseOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Context) : DockerOptions
{
}