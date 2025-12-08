using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("builder")]
[ExcludeFromCodeCoverage]
public record DockerBuilderOptions : DockerOptions
{
}
