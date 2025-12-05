using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("container", "port")]
[ExcludeFromCodeCoverage]
public record DockerContainerPortOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? PrivatePortOrProto { get; set; }
}
