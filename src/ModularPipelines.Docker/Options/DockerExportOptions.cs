using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("export")]
[ExcludeFromCodeCoverage]
public record DockerExportOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions;