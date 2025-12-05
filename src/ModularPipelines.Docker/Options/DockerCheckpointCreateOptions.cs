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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Container { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Checkpoint { get; set; }

    [CliOption("--checkpoint-dir")]
    public virtual string? CheckpointDir { get; set; }

    [CliOption("--leave-running")]
    public virtual string? LeaveRunning { get; set; }
}
