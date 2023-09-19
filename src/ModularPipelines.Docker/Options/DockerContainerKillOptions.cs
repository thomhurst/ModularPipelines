using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container kill")]
[ExcludeFromCodeCoverage]
public record DockerContainerKillOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Containers) : DockerOptions
{
    [CommandSwitch("--signal")]
    public string? Signal { get; set; }
}
