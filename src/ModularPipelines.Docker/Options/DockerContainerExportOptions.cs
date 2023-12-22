using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container export")]
[ExcludeFromCodeCoverage]
public record DockerContainerExportOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions;