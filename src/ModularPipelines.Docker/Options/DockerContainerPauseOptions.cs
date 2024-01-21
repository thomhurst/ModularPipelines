using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "pause")]
[ExcludeFromCodeCoverage]
public record DockerContainerPauseOptions : DockerOptions
{
}
