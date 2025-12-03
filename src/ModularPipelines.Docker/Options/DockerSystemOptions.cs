using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("system")]
[ExcludeFromCodeCoverage]
public record DockerSystemOptions : DockerOptions
{
}
