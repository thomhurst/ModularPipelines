using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service scale")]
public record DockerServiceScaleOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> ServiceReplicas) : DockerOptions
{
    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }
}