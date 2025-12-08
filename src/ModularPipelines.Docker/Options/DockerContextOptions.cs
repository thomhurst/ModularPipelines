using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("context")]
[ExcludeFromCodeCoverage]
public record DockerContextOptions : DockerOptions
{
}
