using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint create")]
public record DockerCheckpointCreateOptions : DockerOptions
{
    [CommandLongSwitch("checkpoint-dir")]
    public string? CheckpointDir { get; set; }

    [CommandLongSwitch("leave-running")]
    public string? LeaveRunning { get; set; }

}