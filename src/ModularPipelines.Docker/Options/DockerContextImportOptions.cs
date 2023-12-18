using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context import")]
[ExcludeFromCodeCoverage]
public record DockerContextImportOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Context, [property: PositionalArgument(Position = Position.AfterSwitches)] string File) : DockerOptions;