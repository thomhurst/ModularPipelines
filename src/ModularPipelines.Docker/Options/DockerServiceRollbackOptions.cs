using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service rollback")]
[ExcludeFromCodeCoverage]
public record DockerServiceRollbackOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Service) : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}
