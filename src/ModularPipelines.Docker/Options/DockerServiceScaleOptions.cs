using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service", "scale")]
[ExcludeFromCodeCoverage]
public record DockerServiceScaleOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<KeyValue>? ServiceReplicas { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }
}
