using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha", "scale")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaScaleOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<KeyValue>? ServiceReplicas { get; set; }

    [BooleanCommandSwitch("--no-deps")]
    public virtual bool? NoDeps { get; set; }
}
