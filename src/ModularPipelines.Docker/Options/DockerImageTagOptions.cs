using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image", "tag")]
[ExcludeFromCodeCoverage]
public record DockerImageTagOptions : DockerOptions
{
}
