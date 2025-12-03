using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSwarmJoinTokenOptions : DockerOptions
{
    public DockerSwarmJoinTokenOptions(
        string workerOrManager
    )
    {
        CommandParts = ["swarm", "join-token"];

        WorkerOrManager = workerOrManager;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? WorkerOrManager { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--rotate")]
    public virtual string? Rotate { get; set; }
}
