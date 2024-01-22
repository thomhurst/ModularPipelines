using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServiceRollbackOptions : DockerOptions
{
    public DockerServiceRollbackOptions(
        string service
    )
    {
        CommandParts = ["service", "rollback"];

        Service = service;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Service { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}
