using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerCheckpointCreateOptions : DockerOptions
{
    public DockerCheckpointCreateOptions(
        string container,
        string checkpoint
    )
    {
        CommandParts = ["checkpoint", "create"];

        Container = container;

        Checkpoint = checkpoint;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Checkpoint { get; set; }

    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }

    [CommandSwitch("--leave-running")]
    public string? LeaveRunning { get; set; }
}
