using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service rollback")]
public record DockerServiceRollbackOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Service) : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

}