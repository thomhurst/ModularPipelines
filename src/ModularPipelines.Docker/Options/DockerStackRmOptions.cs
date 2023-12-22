using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack rm")]
[ExcludeFromCodeCoverage]
public record DockerStackRmOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Stacks) : DockerOptions;