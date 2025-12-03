using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("checkpoint")]
[ExcludeFromCodeCoverage]
public record DockerCheckpointOptions : DockerOptions
{
}
