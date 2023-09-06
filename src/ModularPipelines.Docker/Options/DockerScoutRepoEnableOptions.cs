using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout repo enable")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoEnableOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Repository) : DockerOptions
{
}
