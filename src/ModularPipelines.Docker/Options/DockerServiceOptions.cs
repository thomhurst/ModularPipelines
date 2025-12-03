using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("service")]
[ExcludeFromCodeCoverage]
public record DockerServiceOptions : DockerOptions
{
}
