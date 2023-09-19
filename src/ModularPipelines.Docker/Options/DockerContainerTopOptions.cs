using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container top")]
[ExcludeFromCodeCoverage]
public record DockerContainerTopOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions;
