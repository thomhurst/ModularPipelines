using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint ls")]
public record DockerCheckpointLsOptions : DockerOptions
{
    [CommandLongSwitch("checkpoint-dir")]
    public string? CheckpointDir { get; set; }

}