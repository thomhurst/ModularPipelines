using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container start")]
public record DockerContainerStartOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Container) : DockerOptions
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