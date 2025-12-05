using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("config")]
[ExcludeFromCodeCoverage]
public record DockerConfigOptions : DockerOptions
{
}
