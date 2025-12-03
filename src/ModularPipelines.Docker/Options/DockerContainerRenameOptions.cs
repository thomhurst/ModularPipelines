using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("container", "rename")]
[ExcludeFromCodeCoverage]
public record DockerContainerRenameOptions : DockerOptions
{
}
