using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("save")]
[ExcludeFromCodeCoverage]
public record DockerSaveOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Images) : DockerOptions;
