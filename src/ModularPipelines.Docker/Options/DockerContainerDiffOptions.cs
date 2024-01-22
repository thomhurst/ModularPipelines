using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("container", "diff")]
[ExcludeFromCodeCoverage]
public record DockerContainerDiffOptions : DockerOptions
{
}
