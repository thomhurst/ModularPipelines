using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("service")]
[ExcludeFromCodeCoverage]
public record DockerServiceOptions : DockerOptions
{
}
