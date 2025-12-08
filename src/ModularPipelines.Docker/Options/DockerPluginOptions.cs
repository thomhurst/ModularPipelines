using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("plugin")]
[ExcludeFromCodeCoverage]
public record DockerPluginOptions : DockerOptions
{
}
