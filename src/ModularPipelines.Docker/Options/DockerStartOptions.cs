using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("start")]
[ExcludeFromCodeCoverage]
public record DockerStartOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Containers) : DockerOptions
{
    [BooleanCommandSwitch("--attach")]
    public bool? Attach { get; set; }

    [CommandSwitch("--checkpoint")]
    public string? Checkpoint { get; set; }

    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }

    [CommandSwitch("--detach-keys")]
    public string? DetachKeys { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }
}