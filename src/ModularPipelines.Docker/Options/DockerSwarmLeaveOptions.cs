using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("swarm", "leave")]
[ExcludeFromCodeCoverage]
public record DockerSwarmLeaveOptions : DockerOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
