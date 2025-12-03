using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("plugin")]
[ExcludeFromCodeCoverage]
public record DockerPluginOptions : DockerOptions
{
}
