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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Container { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Checkpoint { get; set; }

    [CliOption("--checkpoint-dir")]
    public virtual string? CheckpointDir { get; set; }
}
