using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "rename")]
[ExcludeFromCodeCoverage]
public record DockerContainerRenameOptions : DockerOptions
{
}
