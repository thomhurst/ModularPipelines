using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container restart")]
[ExcludeFromCodeCoverage]
public record DockerContainerRestartOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Containers) : DockerOptions
{
    [CommandSwitch("--signal")]
    public string? Signal { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }
}
