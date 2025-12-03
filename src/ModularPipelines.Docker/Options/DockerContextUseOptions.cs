using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("context", "use")]
[ExcludeFromCodeCoverage]
public record DockerContextUseOptions : DockerOptions
{
}
