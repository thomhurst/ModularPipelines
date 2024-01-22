using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerStartOptions : DockerOptions
{
    public DockerContainerStartOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "start"];

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Container { get; set; }

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
