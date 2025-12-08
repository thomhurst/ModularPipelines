using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("config")]
[ExcludeFromCodeCoverage]
public record DockerConfigOptions : DockerOptions
{
}
