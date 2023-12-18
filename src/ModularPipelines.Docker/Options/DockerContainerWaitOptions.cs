using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container wait")]
[ExcludeFromCodeCoverage]
public record DockerContainerWaitOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Containers) : DockerOptions;