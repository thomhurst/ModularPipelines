using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("builder")]
[ExcludeFromCodeCoverage]
public record DockerBuilderOptions : DockerOptions
{
}
