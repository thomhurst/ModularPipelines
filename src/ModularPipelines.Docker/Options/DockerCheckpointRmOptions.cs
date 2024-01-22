using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerCheckpointRmOptions : DockerOptions
{
    public DockerCheckpointRmOptions(
        string container,
        string checkpoint
    )
    {
        CommandParts = ["checkpoint", "rm"];

        Container = container;

        Checkpoint = checkpoint;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Checkpoint { get; set; }

    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }
}
