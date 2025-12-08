using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("container")]
[ExcludeFromCodeCoverage]
public record DockerContainerOptions : DockerOptions
{
}
