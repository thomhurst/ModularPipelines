using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("top")]
[ExcludeFromCodeCoverage]
public record DockerTopOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions;
