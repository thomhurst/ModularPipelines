using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual bool? Attach { get; set; }

    [CommandSwitch("--checkpoint")]
    public virtual string? Checkpoint { get; set; }

    [CommandSwitch("--checkpoint-dir")]
    public virtual string? CheckpointDir { get; set; }

    [CommandSwitch("--detach-keys")]
    public virtual string? DetachKeys { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }
}
