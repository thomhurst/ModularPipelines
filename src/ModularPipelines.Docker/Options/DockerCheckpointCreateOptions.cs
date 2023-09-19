using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint create")]
[ExcludeFromCodeCoverage]
public record DockerCheckpointCreateOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container, [property: PositionalArgument(Position = Position.AfterSwitches)] string Checkpoint) : DockerOptions
{
    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }

    [CommandSwitch("--leave-running")]
    public string? LeaveRunning { get; set; }
}
