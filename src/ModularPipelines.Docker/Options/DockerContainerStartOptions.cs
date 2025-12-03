using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerStartOptions : DockerOptions
{
    public DockerContainerStartOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "start"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Container { get; set; }

    [CliFlag("--attach")]
    public virtual bool? Attach { get; set; }

    [CliOption("--checkpoint")]
    public virtual string? Checkpoint { get; set; }

    [CliOption("--checkpoint-dir")]
    public virtual string? CheckpointDir { get; set; }

    [CliOption("--detach-keys")]
    public virtual string? DetachKeys { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }
}
