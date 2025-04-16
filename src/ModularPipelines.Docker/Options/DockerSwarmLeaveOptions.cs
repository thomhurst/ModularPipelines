using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm", "leave")]
[ExcludeFromCodeCoverage]
public record DockerSwarmLeaveOptions : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }
}
