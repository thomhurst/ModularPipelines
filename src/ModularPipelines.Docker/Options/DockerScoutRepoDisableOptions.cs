using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout repo disable")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoDisableOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Repository) : DockerOptions
{
}