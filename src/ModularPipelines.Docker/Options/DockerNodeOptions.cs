using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("node")]
[ExcludeFromCodeCoverage]
public record DockerNodeOptions : DockerOptions
{
}
