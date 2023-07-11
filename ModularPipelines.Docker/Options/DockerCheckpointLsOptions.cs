using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint ls")]
public record DockerCheckpointLsOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }

}