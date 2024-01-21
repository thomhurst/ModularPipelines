using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSwarmJoinTokenOptions : DockerOptions
{
    public DockerSwarmJoinTokenOptions(
        string workerManager
    )
    {
        CommandParts = ["swarm", "join-token"];

        WorkerManager = workerManager;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? WorkerManager { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--rotate")]
    public string? Rotate { get; set; }
}
