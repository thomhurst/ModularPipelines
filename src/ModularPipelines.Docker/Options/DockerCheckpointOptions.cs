using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("checkpoint")]
[ExcludeFromCodeCoverage]
public record DockerCheckpointOptions : DockerOptions
{
}
