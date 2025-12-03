using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("context", "show")]
[ExcludeFromCodeCoverage]
public record DockerContextShowOptions : DockerOptions
{
}
