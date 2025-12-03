using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("container")]
[ExcludeFromCodeCoverage]
public record DockerContainerOptions : DockerOptions
{
}
