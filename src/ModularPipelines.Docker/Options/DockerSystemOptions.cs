using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("system")]
[ExcludeFromCodeCoverage]
public record DockerSystemOptions : DockerOptions
{
}
