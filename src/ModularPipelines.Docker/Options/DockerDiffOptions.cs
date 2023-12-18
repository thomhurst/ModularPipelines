using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("diff")]
[ExcludeFromCodeCoverage]
public record DockerDiffOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions
{
}