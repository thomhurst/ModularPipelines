using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint")]
[ExcludeFromCodeCoverage]
public record DockerCheckpointOptions : DockerOptions
{
}
