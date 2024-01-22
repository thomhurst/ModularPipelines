using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "rename")]
[ExcludeFromCodeCoverage]
public record DockerContainerRenameOptions : DockerOptions
{
}
