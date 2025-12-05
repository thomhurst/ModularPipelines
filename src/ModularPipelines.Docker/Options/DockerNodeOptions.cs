using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("node")]
[ExcludeFromCodeCoverage]
public record DockerNodeOptions : DockerOptions
{
}
