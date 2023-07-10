using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint rm")]
public record DockerCheckpointRmOptions : DockerOptions
{
    [CommandLongSwitch("checkpoint-dir")]
    public string? CheckpointDir { get; set; }

}