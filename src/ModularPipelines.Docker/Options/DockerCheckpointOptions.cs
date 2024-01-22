using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint")]
[ExcludeFromCodeCoverage]
public record DockerCheckpointOptions : DockerOptions
{
}
