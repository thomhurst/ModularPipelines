using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint rm")]
public record DockerCheckpointRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container, [property: PositionalArgument(Position = Position.AfterArguments)] string Checkpoint) : DockerOptions
{

    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }

}