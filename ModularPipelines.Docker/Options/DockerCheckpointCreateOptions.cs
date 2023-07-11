using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint create")]
public record DockerCheckpointCreateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container, [property: PositionalArgument(Position = Position.AfterArguments)] string Checkpoint) : DockerOptions
{

    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }


    [CommandSwitch("--leave-running")]
    public string? LeaveRunning { get; set; }

}