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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? WorkerOrManager { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--rotate")]
    public string? Rotate { get; set; }
}
