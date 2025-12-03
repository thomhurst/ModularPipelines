using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("context")]
[ExcludeFromCodeCoverage]
public record DockerContextOptions : DockerOptions
{
}
