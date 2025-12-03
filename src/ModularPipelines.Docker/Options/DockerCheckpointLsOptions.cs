using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerCheckpointLsOptions : DockerOptions
{
    public DockerCheckpointLsOptions(
        string container
    )
    {
        CommandParts = ["checkpoint", "ls"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Container { get; set; }

    [CliOption("--checkpoint-dir")]
    public virtual string? CheckpointDir { get; set; }
}
