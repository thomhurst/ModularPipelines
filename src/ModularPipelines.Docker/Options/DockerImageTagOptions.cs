using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image", "tag")]
[ExcludeFromCodeCoverage]
public record DockerImageTagOptions : DockerOptions
{
}
