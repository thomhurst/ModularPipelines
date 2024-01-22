using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "pause")]
[ExcludeFromCodeCoverage]
public record DockerComposePauseOptions : DockerOptions
{
}
